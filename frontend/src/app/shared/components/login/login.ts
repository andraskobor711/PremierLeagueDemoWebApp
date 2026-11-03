import { Component, inject } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html'
})
export class Login {
  private readonly auth = inject(AuthService);
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly form = this.formBuilder.group({
    username: ['', [Validators.required, nonWhitespaceValidator]],
    password: ['', Validators.required]
  });
  isSubmitting = false;
  errorMessage: string | null = null;

  submit(): void {
    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;
    const { username, password } = this.form.getRawValue();
    this.auth.login(username, password).subscribe({
      next: () => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
        void this.router.navigateByUrl(this.safeReturnUrl(returnUrl));
      },
      error: () => {
        this.errorMessage = 'Sign-in failed. Check your credentials or try again later.';
        this.isSubmitting = false;
      }
    });
  }

  private safeReturnUrl(returnUrl: string | null): string {
    return returnUrl?.startsWith('/') && !returnUrl.startsWith('//')
      ? returnUrl
      : '/home';
  }
}

const nonWhitespaceValidator: ValidatorFn = control =>
  typeof control.value === 'string' && control.value.trim().length === 0
    ? { required: true }
    : null;
