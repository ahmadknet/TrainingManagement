import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthApiService } from './auth-api.service';

export const authGuard: CanActivateFn = (_route, state) => {
  const authApi = inject(AuthApiService);
  const router = inject(Router);

  if (authApi.isAuthenticated) {
    return true;
  }

  return router.createUrlTree(['/login'], {
    queryParams: {
      returnUrl: state.url
    }
  });
};

export const loginGuard: CanActivateFn = () => {
  const authApi = inject(AuthApiService);
  const router = inject(Router);

  return authApi.isAuthenticated ? router.createUrlTree(['/courses']) : true;
};
