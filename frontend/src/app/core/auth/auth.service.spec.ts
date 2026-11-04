import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

const sessionStorageKey = 'pl-demo-auth-session';

function createToken(payload: Record<string, unknown>): string {
  const encodedPayload = btoa(JSON.stringify(payload))
    .replace(/=/g, '')
    .replace(/\+/g, '-')
    .replace(/\//g, '_');
  return `header.${encodedPayload}.signature`;
}

describe('AuthService', () => {
  let service: AuthService;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    sessionStorage.clear();
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()]
    });
    service = TestBed.inject(AuthService);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTesting.verify();
    sessionStorage.clear();
  });

  it('stores the returned bearer token for the current tab', () => {
    service.login('demo-user', 'password').subscribe();

    const request = httpTesting.expectOne(`${environment.plApiBaseUrl}/auth/login`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({ username: 'demo-user', password: 'password' });
    request.flush({
      accessToken: 'signed-token',
      tokenType: 'Bearer',
      expiresAtUtc: new Date(Date.now() + 60_000).toISOString()
    });

    expect(service.getAccessToken()).toBe('signed-token');
    expect(service.isAuthenticated()).toBe(true);
    expect(sessionStorage.getItem(sessionStorageKey)).not.toBeNull();
  });

  it('removes expired tokens and reports the session as unauthenticated', () => {
    sessionStorage.setItem(sessionStorageKey, JSON.stringify({
      accessToken: 'expired-token',
      expiresAtUtc: new Date(Date.now() - 60_000).toISOString()
    }));

    expect(service.getAccessToken()).toBeNull();
    expect(service.isAuthenticated()).toBe(false);
    expect(sessionStorage.getItem(sessionStorageKey)).toBeNull();
  });

  it('clears malformed saved session data', () => {
    sessionStorage.setItem(sessionStorageKey, '{invalid');

    expect(service.isAuthenticated()).toBe(false);
    expect(sessionStorage.getItem(sessionStorageKey)).toBeNull();
  });

  it('checks true-valued JWT claims and rejects missing or false claims', () => {
    sessionStorage.setItem(sessionStorageKey, JSON.stringify({
      accessToken: createToken({ CanEditMatches: 'true', CanReadMatches: 'false' }),
      expiresAtUtc: new Date(Date.now() + 60_000).toISOString()
    }));

    expect(service.hasClaim('CanEditMatches')).toBe(true);
    expect(service.hasClaim('CanReadMatches')).toBe(false);
    expect(service.hasClaim('CanReadTeams')).toBe(false);
  });

  it('supports array-valued claims and fails closed for malformed JWT payloads', () => {
    sessionStorage.setItem(sessionStorageKey, JSON.stringify({
      accessToken: createToken({ CanEditMatches: ['false', 'true'] }),
      expiresAtUtc: new Date(Date.now() + 60_000).toISOString()
    }));
    expect(service.hasClaim('CanEditMatches')).toBe(true);

    sessionStorage.setItem(sessionStorageKey, JSON.stringify({
      accessToken: 'not-a-jwt',
      expiresAtUtc: new Date(Date.now() + 60_000).toISOString()
    }));
    expect(service.hasClaim('CanEditMatches')).toBe(false);
  });
});
