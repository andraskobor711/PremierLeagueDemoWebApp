import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

const sessionStorageKey = 'pl-demo-auth-session';

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
});
