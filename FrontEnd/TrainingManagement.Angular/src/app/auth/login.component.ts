import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';

import { AuthApiService } from './auth-api.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  protected authMode: 'login' | 'register' = 'login';
  protected loginForm = {
    email: '',
    password: ''
  };
  protected registerForm = {
    email: '',
    password: '',
    confirmPassword: ''
  };
  protected authenticating = false;
  protected authError = '';

  constructor(
    private readonly authApi: AuthApiService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  protected setAuthMode(mode: 'login' | 'register'): void {
    this.authMode = mode;
    this.authError = '';
  }

  protected login(): void {
    this.authError = '';

    const email = this.loginForm.email.trim();
    const password = this.loginForm.password;

    if (!email || !password) {
      this.authError = 'Enter your email and password.';
      return;
    }

    this.authenticating = true;

    this.authApi
      .login({ email, password })
      .pipe(finalize(() => (this.authenticating = false)))
      .subscribe({
        next: (response) => {
          if (!response.success) {
            this.authError = response.message || 'Login failed.';
            return;
          }

          this.navigateAfterLogin();
        },
        error: (err) => {
          this.authError = this.extractHttpError(err, 'Login failed.');
        }
      });
  }

  protected register(): void {
    this.authError = '';

    const email = this.registerForm.email.trim();
    const password = this.registerForm.password;
    const confirmPassword = this.registerForm.confirmPassword;

    if (!email || !password || !confirmPassword) {
      this.authError = 'Enter an email, password, and confirmation password.';
      return;
    }

    if (password !== confirmPassword) {
      this.authError = 'Passwords do not match.';
      return;
    }

    this.authenticating = true;

    this.authApi
      .register({ email, password, confirmPassword })
      .pipe(finalize(() => (this.authenticating = false)))
      .subscribe({
        next: (response) => {
          if (!response.success) {
            this.authError = response.message || 'Account could not be created.';
            return;
          }

          this.navigateAfterLogin();
        },
        error: (err) => {
          this.authError = this.extractHttpError(err, 'Account could not be created.');
        }
      });
  }

  private navigateAfterLogin(): void {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
    this.router.navigateByUrl(returnUrl && returnUrl !== '/login' ? returnUrl : '/courses');
  }

  private extractHttpError(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.message || fallback;
    }

    return fallback;
  }
}
