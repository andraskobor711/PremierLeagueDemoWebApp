import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthService } from '../../../core/auth/auth.service';
import { Home } from './home';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;
  const authService = {
    hasClaim: vi.fn((claim: string) => claim === 'CanReadMatches'),
  };

  beforeEach(async () => {
    authService.hasClaim.mockImplementation(claim => claim === 'CanReadMatches');
    await TestBed.configureTestingModule({
      imports: [Home],
      providers: [{ provide: AuthService, useValue: authService }],
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('shows only sections granted by read claims', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('.home-button--matches')).not.toBeNull();
    expect(compiled.querySelector('.home-button--teams')).toBeNull();
    expect(compiled.querySelector('.home-button--tables')).toBeNull();
  });
});
