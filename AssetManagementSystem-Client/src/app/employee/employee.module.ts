import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Employee } from './employee';
import { ViewEmployeeList } from '../pages/view-employee-list/view-employee-list';

const routes: Routes = [
  { path: '', component: ViewEmployeeList },
  { path: 'add', component: Employee },
];

@NgModule({
  imports: [RouterModule.forChild(routes), Employee, ViewEmployeeList],
  exports: [RouterModule],
})
export class EmployeeModule {}