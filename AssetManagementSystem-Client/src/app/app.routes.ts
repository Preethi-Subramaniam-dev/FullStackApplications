import { Routes } from '@angular/router';
import { Assets } from './pages/assets/assets';
import { DashboardPage } from './pages/dashboard/dashboard';
import { SoftwarePage } from './pages/software/software';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Assets },
  { path: 'assets', component: Assets },
  {
    path: 'employees',
    loadChildren: () => import('./employee/employee.module').then(m => m.EmployeeModule),
  },
  { path: 'software', component: SoftwarePage },
];
