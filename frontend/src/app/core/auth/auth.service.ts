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

type JwtClaims = Record<string, unknown>;

@Service()
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly storageKey = 'pl-demo-auth-session';
  private decodedClaimsToken: string | null = null;
  private decodedClaims: JwtClaims | null = null;

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

  hasClaim(claim: string): boolean {
    const token = this.getAccessToken();
    if (!token) {
      return false;
    }

    const claims = this.readTokenClaims(token);
    const value = claims?.[claim];
    return value === 'true' || (Array.isArray(value) && value.includes('true'));
  }

  logout(): void {
    sessionStorage.removeItem(this.storageKey);
    this.decodedClaimsToken = null;
    this.decodedClaims = null;
  }

  private readTokenClaims(token: string): JwtClaims | null {
    if (this.decodedClaimsToken === token) {
      return this.decodedClaims;
    }

    this.decodedClaimsToken = token;
    this.decodedClaims = null;
    const payload = token.split('.')[1];
    if (!payload) {
      return null;
    }

    try {
      const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
      const paddedBase64 = base64.padEnd(Math.ceil(base64.length / 4) * 4, '=');
      const bytes = Uint8Array.from(atob(paddedBase64), character => character.charCodeAt(0));
      const parsed: unknown = JSON.parse(new TextDecoder().decode(bytes));
      if (typeof parsed === 'object' && parsed !== null && !Array.isArray(parsed)) {
        this.decodedClaims = parsed as JwtClaims;
      }
    } catch {
      return null;
    }

    return this.decodedClaims;
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
