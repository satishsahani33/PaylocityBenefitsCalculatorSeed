import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PayCheckRoutingModule } from './pay-check-routing.module';
import { PayCheckComponent } from '../pay-check/pay-check/pay-check.component';
import { ListPayCheckComponent } from './list-pay-check/list-pay-check.component';
import { ViewPayCheckComponent } from './view-pay-check/view-pay-check.component';
import { CalculatePayCheckComponent } from './calculate-pay-check/calculate-pay-check.component';
import { ReactiveFormsModule } from '@angular/forms';
import { PayCheckConfirmationDialogComponent } from './pay-check-confirmation-dialog/pay-check-confirmation-dialog.component';


@NgModule({
  declarations: [
    PayCheckComponent,
    ListPayCheckComponent,
    ViewPayCheckComponent,
    CalculatePayCheckComponent,
    PayCheckConfirmationDialogComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    PayCheckRoutingModule
  ]
})
export class PayCheckModule { }
