import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthApiService } from './auth/auth-api.service';
import { AuthSession } from './auth/auth.models';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit, OnDestroy {
  protected session: AuthSession | null = null;
  private sessionSubscription?: Subscription;

  constructor(
    private readonly authApi: AuthApiService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.session = this.authApi.session;
    this.sessionSubscription = this.authApi.session$.subscribe((session) => {
      this.session = session;
    });
  }

  ngOnDestroy(): void {
    this.sessionSubscription?.unsubscribe();
  }

  protected get isAuthenticated(): boolean {
    return !!this.session?.accessToken;
  }

  protected get currentUserEmail(): string {
    return this.session?.user?.email ?? 'Signed in';
  }

  protected logout(): void {
    this.authApi.logout();
    this.router.navigate(['/login']);
  }
}
