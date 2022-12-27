import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PayCheckComponent } from '../pay-check/pay-check/pay-check.component';
import { ListPayCheckComponent } from './list-pay-check/list-pay-check.component';
import { ViewPayCheckComponent } from './view-pay-check/view-pay-check.component';
import { CalculatePayCheckComponent } from './calculate-pay-check/calculate-pay-check.component';

const routes: Routes = [
  {
    path: '', component: PayCheckComponent,
    children: [
      { path: '', component: ListPayCheckComponent },
      {path: 'payCheck', component: PayCheckComponent},
      { path: 'calculate/:id', component: CalculatePayCheckComponent },
      { path: 'view/:id', component: ViewPayCheckComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PayCheckRoutingModule { }
