import { TestBed } from '@angular/core/testing';
import { provideRouter, Router, UrlTree } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { claimGuard } from './claim.guard';

describe('claimGuard', () => {
  const authService = {
    hasClaim: vi.fn((claim: string) => claim === 'CanEditMatches')
  };

  beforeEach(() => {
    authService.hasClaim.mockImplementation(claim => claim === 'CanEditMatches');
    TestBed.configureTestingModule({
      providers: [provideRouter([]), { provide: AuthService, useValue: authService }]
    });
  });

  it('allows activation only when every required claim is present', () => {
    const guard = claimGuard('CanEditMatches', 'CanReadMatches');
    authService.hasClaim.mockReturnValue(true);

    const result = TestBed.runInInjectionContext(() => guard(null!, null!));

    expect(result).toBe(true);
    expect(authService.hasClaim).toHaveBeenCalledWith('CanEditMatches');
    expect(authService.hasClaim).toHaveBeenCalledWith('CanReadMatches');
  });

  it('redirects to forbidden when a required claim is absent', () => {
    const guard = claimGuard('CanEditMatches', 'CanReadMatches');
    authService.hasClaim.mockImplementation(claim => claim === 'CanEditMatches');

    const result = TestBed.runInInjectionContext(() => guard(null!, null!));

    expect(result).toBeInstanceOf(UrlTree);
    expect(TestBed.inject(Router).serializeUrl(result as UrlTree)).toBe('/forbidden');
  });
});
