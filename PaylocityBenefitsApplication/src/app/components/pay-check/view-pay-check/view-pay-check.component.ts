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
  loading = false;
  payCheckInfo?: any;

  constructor(
      private route: ActivatedRoute,
      private payCheckService: PayCheckService
  ) {}

  ngOnInit() {
      this.id = this.route.snapshot.params['id'];
      if (this.id) {
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
