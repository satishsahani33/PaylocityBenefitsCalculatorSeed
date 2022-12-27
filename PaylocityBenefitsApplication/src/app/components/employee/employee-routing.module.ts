import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeComponent } from './employee/employee.component';
import { AddEditEmployeeComponent } from './add-edit-employee/add-edit-employee.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { ViewPayCheckComponent } from '../pay-check/view-pay-check/view-pay-check.component';
import { CalculatePayCheckComponent } from '../pay-check/calculate-pay-check/calculate-pay-check.component';
import { DependentListComponent } from '../dependent/dependent-list/dependent-list.component';
import { ListPayCheckComponent } from '../pay-check/list-pay-check/list-pay-check.component';

const routes: Routes = [
  {
      path: '', component: EmployeeComponent,
      children: [
        { path: '', component: EmployeeListComponent },
        { path: 'add', component: AddEmployeeComponent },
        { path: 'edit/:id', component: AddEditEmployeeComponent },
        { path: 'dependents/:id', component: DependentListComponent },
        { path: 'payChecks/:id', component: ListPayCheckComponent },
        { path: 'viewPayCheck/:id', component: ViewPayCheckComponent },
        { path: 'calculatePayCheck/:id/:year/:month', component: CalculatePayCheckComponent }
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRoutingModule { }
