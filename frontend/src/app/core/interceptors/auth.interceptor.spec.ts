import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { environment } from '../../../environments/environment';
import { authInterceptor } from './auth.interceptor';

const sessionStorageKey = 'pl-demo-auth-session';

describe('authInterceptor', () => {
  let httpTesting: HttpTestingController;
  let router: Router;
  let http: HttpClient;

  beforeEach(() => {
    sessionStorage.clear();
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    });
    httpTesting = TestBed.inject(HttpTestingController);
    router = TestBed.inject(Router);
    http = TestBed.inject(HttpClient);
    vi.spyOn(router, 'navigate').mockResolvedValue(true);
  });

  afterEach(() => {
    httpTesting.verify();
    sessionStorage.clear();
  });

  it('adds a bearer token only to requests sent to the configured API', () => {
    saveSession();

    http.get(`${environment.plApiBaseUrl}/teams`).subscribe();
    const request = httpTesting.expectOne(`${environment.plApiBaseUrl}/teams`);
    expect(request.request.headers.get('Authorization')).toBe('Bearer signed-token');
    request.flush([]);

    http.get('https://example.test/data').subscribe();
    const external = httpTesting.expectOne('https://example.test/data');
    expect(external.request.headers.has('Authorization')).toBe(false);
    external.flush({});
  });

  it('clears the session and redirects to login on an API 401', () => {
    saveSession();

    http.get(`${environment.plApiBaseUrl}/teams`).subscribe({ error: () => undefined });
    httpTesting.expectOne(`${environment.plApiBaseUrl}/teams`).flush(
      {},
      { status: 401, statusText: 'Unauthorized' }
    );

    expect(sessionStorage.getItem(sessionStorageKey)).toBeNull();
    expect(router.navigate).toHaveBeenCalledWith(['/login'], {
      queryParams: { returnUrl: '/' }
    });
  });

  it('keeps the session and redirects to forbidden on an API 403', () => {
    saveSession();

    http.get(`${environment.plApiBaseUrl}/teams`).subscribe({ error: () => undefined });
    httpTesting.expectOne(`${environment.plApiBaseUrl}/teams`).flush(
      {},
      { status: 403, statusText: 'Forbidden' }
    );

    expect(sessionStorage.getItem(sessionStorageKey)).not.toBeNull();
    expect(router.navigate).toHaveBeenCalledWith(['/forbidden']);
  });

  it('does not redirect when login itself returns 401', () => {
    http.post(`${environment.plApiBaseUrl}/auth/login`, {})
      .subscribe({ error: () => undefined });
    httpTesting.expectOne(`${environment.plApiBaseUrl}/auth/login`).flush(
      {},
      { status: 401, statusText: 'Unauthorized' }
    );

    expect(router.navigate).not.toHaveBeenCalled();
  });
});

function saveSession(): void {
  sessionStorage.setItem(sessionStorageKey, JSON.stringify({
    accessToken: 'signed-token',
    expiresAtUtc: new Date(Date.now() + 60_000).toISOString()
  }));
}
