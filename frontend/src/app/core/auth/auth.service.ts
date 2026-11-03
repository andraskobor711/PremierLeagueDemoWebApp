import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface LoginResponse {
  accessToken: string;
  tokenType: string;
  expiresAtUtc: string;
}

interface StoredSession {
  accessToken: string;
  expiresAtUtc: string;
}

@Service()
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly storageKey = 'pl-demo-auth-session';

  login(username: string, password: string): Observable<void> {
    return this.http
      .post<LoginResponse>(`${environment.plApiBaseUrl}/auth/login`, { username, password })
      .pipe(
        tap(response => {
          const expiresAt = Date.parse(response.expiresAtUtc);
          if (
            !response.accessToken ||
            response.tokenType.toLowerCase() !== 'bearer' ||
            !Number.isFinite(expiresAt) ||
            expiresAt <= Date.now()
          ) {
            throw new Error('The authentication server returned an invalid access token.');
          }

          const session: StoredSession = {
            accessToken: response.accessToken,
            expiresAtUtc: response.expiresAtUtc
          };
          sessionStorage.setItem(this.storageKey, JSON.stringify(session));
        }),
        map(() => undefined)
      );
  }

  getAccessToken(): string | null {
    const session = this.readSession();
    if (!session) {
      return null;
    }

    if (Date.parse(session.expiresAtUtc) <= Date.now()) {
      this.logout();
      return null;
    }

    return session.accessToken;
  }

  isAuthenticated(): boolean {
    return this.getAccessToken() !== null;
  }

  logout(): void {
    sessionStorage.removeItem(this.storageKey);
  }

  private readSession(): StoredSession | null {
    const storedValue = sessionStorage.getItem(this.storageKey);
    if (storedValue === null) {
      return null;
    }

    let value: unknown;
    try {
      value = JSON.parse(storedValue);
    } catch {
      this.logout();
      return null;
    }

    if (
      typeof value === 'object' &&
      value !== null &&
      'accessToken' in value &&
      typeof value.accessToken === 'string' &&
      value.accessToken.length > 0 &&
      'expiresAtUtc' in value &&
      typeof value.expiresAtUtc === 'string' &&
      Number.isFinite(Date.parse(value.expiresAtUtc))
    ) {
      return { accessToken: value.accessToken, expiresAtUtc: value.expiresAtUtc };
    }

    this.logout();
    return null;
  }
}
