import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Home } from './pages/home/home';
import { ForgotPassword } from './pages/forgot-password/forgot-password';
import { ResetPassword } from './pages/reset-password/reset-password';
import { Landing } from './pages/landing/landing';

import { AdminUsers } from './pages/admin-users/admin-users';
import { AdminMediators } from './pages/admin-mediators/admin-mediators';
import { AdminCases } from './pages/admin-cases/admin-cases';
import { AdminReports } from './pages/admin-reports/admin-reports';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';

import { MyCases } from './pages/citizen/my-cases/my-cases';
import { NewCase } from './pages/citizen/new-case/new-case';
import { Agreements } from './pages/citizen/agreements/agreements';
import { Compliance } from './pages/citizen/compliance/compliance';

import { AssignedCases } from './pages/mediator/assigned-cases/assigned-cases';
import { CaseDetail } from './pages/mediator/case-detail/case-detail';
import { SessionForm } from './pages/mediator/session-form/session-form';
import { AgreementForm } from './pages/mediator/agreement-form/agreement-form';

export const routes: Routes = [
  {
    path: '',
    component: Landing,
  },
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

  // Rutas del administrador
  {
    path: 'admin/dashboard',
    component: AdminDashboard,
  },
  {
    path: 'admin/users',
    component: AdminUsers,
  },
  {
    path: 'admin/mediators',
    component: AdminMediators,
  },
  {
    path: 'admin/cases',
    component: AdminCases,
  },
  {
    path: 'admin/reports',
    component: AdminReports,
  },

  // Rutas del ciudadano
  {
    path: 'citizen/new-case',
    component: NewCase,
  },
  {
    path: 'citizen/my-cases',
    component: MyCases,
  },
  {
    path: 'citizen/agreements',
    component: Agreements,
  },
  {
    path: 'citizen/compliance',
    component: Compliance,
  },

  // Rutas del mediador
  {
    path: 'mediator/cases',
    component: AssignedCases,
  },
  {
    path: 'mediator/cases/:id',
    component: CaseDetail,
  },
  {
    path: 'mediator/sessions',
    component: SessionForm,
  },
  {
    path: 'mediator/agreements',
    component: AgreementForm,
  },

  {
    path: '**',
    redirectTo: '',
  },
];