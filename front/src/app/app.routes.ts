import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

import { AuthGuard } from './auth/AuthGuard';
import { LoginGuard } from './auth/LoginGuard';

export const routes: Routes = [
  { path: '', component: DashboardComponent, canActivate: [AuthGuard] },

  { path: 'auth/login', component: LoginComponent, canActivate: [LoginGuard] },
  {
    path: 'auth/register',
    component: RegisterComponent,
    canActivate: [LoginGuard],
  },
];
