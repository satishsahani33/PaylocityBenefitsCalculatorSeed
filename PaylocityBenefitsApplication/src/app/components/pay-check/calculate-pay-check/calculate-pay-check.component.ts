import { ThisReceiver } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PayCheckService, AlertService } from '@app/services';
import { first } from 'rxjs';

@Component({
  selector: 'app-calculate-pay-check',
  templateUrl: './calculate-pay-check.component.html'
})
export class CalculatePayCheckComponent implements OnInit {
  id?: string;
  year?: string;
  month?: string;
  title!: string;
  loading = false;
  submitting = false;
  submitted = false;
  constructor(
      private route: ActivatedRoute,
      private router: Router,
      private payCheckService: PayCheckService,
      private alertService: AlertService
  ) {}

  ngOnInit() {
      this.id = this.route.snapshot.params['id'];
      this.year = this.route.snapshot.params['year'];
      this.month = this.route.snapshot.params['month'];
      if (this.id && this.year && this.month) {
          this.loading = true;
          this.payCheckService.create(this.id, this.year, this.month)
              .pipe(first())
              .subscribe({
                next: (response: any) => {
                  if(response.success){
                    console.log('Pay check calculated for employee ' + this.id);
                    this.alertService.success('Pay Check Calculated', { keepAfterRouteChange: true });
                    this.router.navigateByUrl('/employee/payChecks/' + this.id);
                  }else{
                    this.alertService.error(response.message, { keepAfterRouteChange: true });
                  }
                },
                error: error => {
                    this.submitting = false;
                }
            });
      }
  }
}
