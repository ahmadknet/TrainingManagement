import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { environment } from '../../environments/environment';
import { AuthResponse, AuthSession, LoginRequest, RegisterRequest } from './auth.models';

const SESSION_STORAGE_KEY = 'training-management-auth-session';

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {
  private readonly authUrl = `${environment.authApiBaseUrl}/api/Auth`;
  private readonly sessionSubject = new BehaviorSubject<AuthSession | null>(this.readSession());

  readonly session$ = this.sessionSubject.asObservable();

  constructor(private readonly http: HttpClient) {}

  get session(): AuthSession | null {
    return this.sessionSubject.value;
  }

  get isAuthenticated(): boolean {
    return !!this.session?.accessToken;
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.authUrl}/login`, request)
      .pipe(tap((response) => this.storeSessionFromResponse(response)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.authUrl}/register`, request)
      .pipe(tap((response) => this.storeSessionFromResponse(response)));
  }

  logout(): void {
    this.sessionSubject.next(null);

    if (this.canUseStorage()) {
      localStorage.removeItem(SESSION_STORAGE_KEY);
    }
  }

  private storeSessionFromResponse(response: AuthResponse): void {
    if (!response.success || !response.accessToken) {
      return;
    }

    const session: AuthSession = {
      accessToken: response.accessToken,
      user: response.user
    };

    this.sessionSubject.next(session);

    if (this.canUseStorage()) {
      localStorage.setItem(SESSION_STORAGE_KEY, JSON.stringify(session));
    }
  }

  private readSession(): AuthSession | null {
    if (!this.canUseStorage()) {
      return null;
    }

    const rawSession = localStorage.getItem(SESSION_STORAGE_KEY);
    if (!rawSession) {
      return null;
    }

    try {
      const session = JSON.parse(rawSession) as AuthSession;
      return session.accessToken ? session : null;
    } catch {
      localStorage.removeItem(SESSION_STORAGE_KEY);
      return null;
    }
  }

  private canUseStorage(): boolean {
    return typeof localStorage !== 'undefined';
  }
}
