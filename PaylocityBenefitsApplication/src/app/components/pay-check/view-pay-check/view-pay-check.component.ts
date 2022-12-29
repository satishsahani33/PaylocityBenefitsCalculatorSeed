import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService, AlertService, PayCheckService } from '@app/services';
import { first } from 'rxjs';

@Component({
  selector: 'app-view-pay-check',
  templateUrl: './view-pay-check.component.html',
  styleUrls: ['./view-pay-check.component.css']
})
export class ViewPayCheckComponent implements OnInit {
  id?: string;
  year?: string;
  month?: string;
  loading = false;
  payCheckInfo?: any;

  constructor(
      private route: ActivatedRoute,
      private payCheckService: PayCheckService,
      private alertService: AlertService
  ) {}

  ngOnInit() {
      this.id = this.route.snapshot.params['id'];
      this.year = this.route.snapshot.params['year'];
      this.month = this.route.snapshot.params['month'];
      if (this.id) {
        if (this.year && this.month) {
          this.loading = true;
          this.payCheckService.viewPayCheckOfEmployee(this.id, this.year, this.month)
              .pipe(first())
              .subscribe(x => {
                if(!x.success && x.message){
                  this.alertService.error(x.message);
                }
                  this.payCheckInfo = x.data;
                  if(this.payCheckInfo.employeeId == 0){
                    this.payCheckInfo.employeeId = this.id;
                  }
                  this.loading = false;
              });
        }else{
          this.loading = true;
          this.payCheckService.getById(this.id)
              .pipe(first())
              .subscribe(x => {
                  this.payCheckInfo = x.data;
                  this.loading = false;
              });
        }
      }
  }
}
