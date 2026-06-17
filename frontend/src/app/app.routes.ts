import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Home } from './pages/home/home';
import { ForgotPassword } from './pages/forgot-password/forgot-password';
import { ResetPassword } from './pages/reset-password/reset-password';
import { AdminUsers } from './pages/admin-users/admin-users';
import { MyCases } from './pages/citizen/my-cases/my-cases';
import { NewCase } from './pages/citizen/new-case/new-case';
import { Agreements } from './pages/citizen/agreements/agreements';
import { Compliance } from './pages/citizen/compliance/compliance';

export const routes: Routes = [
  {
    path: 'login',
    component: Login,
  },
  {
    path: 'register',
    component: Register,
  },
  {
    path: 'home',
    component: Home,
  },
  {
    path: 'forgot-password',
    component: ForgotPassword,
  },
  {
    path: 'reset-password',
    component: ResetPassword,
  },

  {
    path: 'citizen/new-case',
    component: NewCase,
  },
  {
    path: 'citizen/agreements',
    component: Agreements,
  },
  {
    path: 'citizen/compliance',
    component: Compliance,
  },
  {
    path: 'citizen/my-cases',
    component: MyCases,
  },
  {
  path: 'admin/users',
  component: AdminUsers,
},

  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full',
  },

  
];
