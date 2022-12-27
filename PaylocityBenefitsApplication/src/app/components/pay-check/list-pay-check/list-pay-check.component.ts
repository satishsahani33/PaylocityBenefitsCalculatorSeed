import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService, PayCheckService } from '@app/services';
import { first } from 'rxjs';

@Component({
  selector: 'app-list-pay-check',
  templateUrl: './list-pay-check.component.html'
})
export class ListPayCheckComponent implements OnInit {
  id?: string;
  loading = false;
  payChecks?: any[];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private payCheckService: PayCheckService,
    private alertService: AlertService,
    private router: Router) {}
  
  ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    if (this.id) {
        this.loading = true;
        this.payCheckService.getAllPayChecksOfEmployee(this.id)
            .pipe(first())
            .subscribe(x => {
                this.payChecks = x.data;
                this.loading = false;
            });
    }else{
      this.payCheckService.getAll()
          .pipe(first())
          .subscribe(payChecks => this.payChecks = payChecks.data);
    }
  }

  deletePayCheck(id: string) {
      const paycheck = this.payChecks!.find(x => x.id === id);
      paycheck.isDeleting = true;
      this.payCheckService.delete(id)
          .pipe(first())
          .subscribe(() => {this.payChecks = this.payChecks!.filter(x => x.id !== id);
            this.alertService.success('Pay Check deleted', { keepAfterRouteChange: true });});
  }
  public openConfirmationDialog(id: string) {
    this.alertService.confirm('Please confirm', 'Are you sure you want to delete?')
    .then((confirmed) => 
    {
      if(confirmed){
        console.log('Pay Check deleted, id= :', id);
        this.deletePayCheck(id);
      }else{
        console.log('User dismissed the dialog');
      }
    })
    .catch(() => console.log('User dismissed the dialog'));
  }
  payCheckTitle?: string;

  openCalculatePayCheckConfirmation(id: string, title:string) {
    this.payCheckTitle = title;
    this.alertService.calculatePayCheckConfirmation('Please select','Are you sure?')
    .then((result:any) => 
    {
      if(result){
         // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (!result.payCheckYear && !result.payCheckMonth) {
          this.alertService.error('Please select both year and month.');
            return;
        }
        this.router.navigateByUrl('/employee/calculatePayCheck/'+ id +'/'+result.payCheckYear + '/'+result.payCheckMonth);
        console.log('Pay check will be calculated for employee id ' + id + ' of year = ' + 
        result.payCheckYear + ' and month = ' + result.payCheckMonth);
      }
    })
    .catch(() => console.log('User dismissed the dialog'));
  }
}
