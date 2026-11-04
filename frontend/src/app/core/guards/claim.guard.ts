import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

export const claimGuard = (...requiredClaims: string[]): CanActivateFn => () => {
  const auth = inject(AuthService);
  if (requiredClaims.every(claim => auth.hasClaim(claim))) {
    return true;
  }

  return inject(Router).createUrlTree(['/forbidden']);
};
