import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Login } from './login';

describe('Login', () => {
  let fixture: ComponentFixture<Login>;
  let httpTesting: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Login],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    httpTesting = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
  });

  afterEach(() => httpTesting.verify());

  it('immediately shows required-field errors without sending a login request', () => {
    fixture.componentInstance.submit();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('#username-error')?.textContent).toContain('Enter a username.');
    expect(compiled.querySelector('#password-error')?.textContent).toContain('Enter a password.');
    expect(compiled.querySelector('#username')?.getAttribute('aria-invalid')).toBe('true');
    expect(compiled.querySelector('#password')?.getAttribute('aria-invalid')).toBe('true');
  });

  it('rejects a username containing only whitespace', () => {
    fixture.componentInstance.form.controls.username.setValue('   ');

    expect(fixture.componentInstance.form.controls.username.hasError('required')).toBe(true);
  });
});
