import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DependentService, AlertService } from '@app/services';
import { first } from 'rxjs';

@Component({
  selector: 'app-add-dependent',
  templateUrl: './add-dependent.component.html',
  styleUrls: ['./add-dependent.component.css']
})
export class AddDependentComponent  implements OnInit {
  form!: FormGroup;
  employeeId?: string;
  title!: string;
  loading = false;
  submitting = false;
  submitted = false;

  constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private dependentService: DependentService,
      private alertService: AlertService
  ) {}

  ngOnInit() {
      this.employeeId = this.route.snapshot.params['id'];
      if(!this.employeeId)
      this.router.navigateByUrl('/employee');
      this.form = this.formBuilder.group({
          employeeId:[this.employeeId],
          firstName: ['', Validators.required],
          lastName: ['', Validators.required],
          relationship: ['', Validators.required],
          dateOfBirth: ['', [Validators.required, Validators.pattern(/^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$/)]],
      });

      this.title = 'Add Dependent';
      if (this.employeeId) {
        console.log('Before Adding New dependent for employee id = ', this.employeeId);
      }
  }
  // format date in typescript
  getFormatedDate(date: Date, format: string) {
    const datePipe = new DatePipe('en-US');
    return datePipe.transform(date, format);
  }
  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
      this.submitted = true;

      // reset alerts on submit
      this.alertService.clear();

      // stop here if form is invalid
      if (this.form.invalid) {
          return;
      }

      this.submitting = true;
      this.saveDependent()
          .pipe(first())
          .subscribe({
              next: (response:any) => {
                if(response.success){
                  this.alertService.success('Dependent saved', { keepAfterRouteChange: true });
                  console.log('New dependent added for employee id = ', this.employeeId);
                  this.router.navigateByUrl('/employee/dependents/'+this.employeeId);
                }
                else{
                  this.alertService.error(response.message, { keepAfterRouteChange: true });
                }
              },
              error: error => {
                  this.submitting = false;
              }
          })
  }

  private saveDependent() {
    this.form.value.relationship = Number(this.form.value.relationship);
      // create or update Dependent based on id param
      return this.dependentService.create(this.form.value);
  }
}
