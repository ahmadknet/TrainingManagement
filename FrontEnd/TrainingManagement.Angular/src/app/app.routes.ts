import { Routes } from '@angular/router';

import { authGuard, loginGuard } from './auth/auth.guard';
import { LoginComponent } from './auth/login.component';
import { CoursesComponent } from './courses/courses.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [loginGuard]
  },
  {
    path: 'courses',
    component: CoursesComponent,
    canActivate: [authGuard]
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'courses'
  },
  {
    path: '**',
    redirectTo: 'courses'
  }
];
