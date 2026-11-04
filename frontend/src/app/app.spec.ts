import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { routes } from './app.routes';
import { AuthService } from './core/auth/auth.service';

describe('App', () => {
  const authService = {
    isAuthenticated: vi.fn(() => true),
    hasClaim: vi.fn((claim: string) => claim === 'CanReadMatches'),
    logout: vi.fn()
  };

  beforeEach(async () => {
    authService.isAuthenticated.mockReturnValue(true);
    authService.hasClaim.mockImplementation(claim => claim === 'CanReadMatches');
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideHttpClient(),
        provideRouter(routes),
        { provide: AuthService, useValue: authService }
      ]
    })
      .compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render the application navigation', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('nav[aria-label="Main navigation"]')).not.toBeNull();
  });

  it('shows only navigation sections granted by read claims', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    await fixture.whenStable();
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('.nav-link--matches')).not.toBeNull();
    expect(compiled.querySelector('.nav-link--teams')).toBeNull();
    expect(compiled.querySelector('.nav-link--tables')).toBeNull();
  });

  it('shows the read-only notice to authenticated users without match-edit permission', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    await fixture.whenStable();

    expect(fixture.nativeElement.querySelector('.read-only-notice')?.textContent)
      .toContain('run the backend locally and sign in with an admin account');
  });

  it('hides the read-only notice from users with match-edit permission', async () => {
    authService.hasClaim.mockImplementation(claim => claim === 'CanEditMatches');
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    await fixture.whenStable();

    expect(fixture.nativeElement.querySelector('.read-only-notice')).toBeNull();
  });

  it('hides the read-only notice when unauthenticated', async () => {
    authService.isAuthenticated.mockReturnValue(false);
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    await fixture.whenStable();

    expect(fixture.nativeElement.querySelector('.read-only-notice')).toBeNull();
  });
});
