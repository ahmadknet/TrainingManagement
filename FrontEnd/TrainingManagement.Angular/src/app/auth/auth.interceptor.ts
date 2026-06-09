import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { environment } from '../../environments/environment';
import { AuthApiService } from './auth-api.service';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authApi = inject(AuthApiService);
  const token = authApi.session?.accessToken;
  const isCourseApiRequest = request.url.startsWith(environment.courseApiBaseUrl);

  if (!token || !isCourseApiRequest) {
    return next(request);
  }

  return next(
    request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
  );
};
