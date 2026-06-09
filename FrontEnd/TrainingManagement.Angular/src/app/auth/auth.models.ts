export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest extends LoginRequest {
  confirmPassword: string;
}

export interface AuthUser {
  id?: number;
  email?: string;
}

export interface AuthResponse {
  success: boolean;
  message?: string;
  accessToken?: string;
  refreshToken?: string;
  user?: AuthUser;
}

export interface AuthSession {
  accessToken: string;
  user?: AuthUser;
}
