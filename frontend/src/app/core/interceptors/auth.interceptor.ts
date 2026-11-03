import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const apiBaseUrl = new URL(environment.plApiBaseUrl);
  const requestUrl = new URL(request.url, apiBaseUrl);
  const basePath = apiBaseUrl.pathname.replace(/\/+$/, '');
  const isApiRequest =
    requestUrl.origin === apiBaseUrl.origin &&
    (basePath === '' ||
      requestUrl.pathname === basePath ||
      requestUrl.pathname.startsWith(`${basePath}/`));
  const loginUrl = new URL(`${basePath}/auth/login`, apiBaseUrl.origin);
  const isLoginRequest = isApiRequest && requestUrl.pathname === loginUrl.pathname;
  const auth = inject(AuthService);
  const router = inject(Router);
  const token = isApiRequest && !isLoginRequest ? auth.getAccessToken() : null;
  const outgoingRequest = token
    ? request.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : request;

  return next(outgoingRequest).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && isApiRequest) {
        if (error.status === 401 && !isLoginRequest) {
          auth.logout();
          if (!router.url.startsWith('/login')) {
            void router.navigate(['/login'], {
              queryParams: { returnUrl: router.url }
            });
          }
        } else if (error.status === 403 && !isLoginRequest) {
          void router.navigate(['/forbidden']);
        }
      }

      return throwError(() => error);
    })
  );
};
