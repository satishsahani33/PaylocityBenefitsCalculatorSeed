import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from '@app/models';
import { EmployeeService,AlertService } from '@app/services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.css']
})
export class AddEditEmployeeComponent implements OnInit {
  form!: FormGroup;
  id?: string;
  title!: string;
  loading = false;
  submitting = false;
  submitted = false;

  constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private employeeService: EmployeeService,
      private alertService: AlertService
  ) {}

  ngOnInit() {
      this.id = this.route.snapshot.params['id'];
      
      this.form = this.formBuilder.group({
          firstName: ['', Validators.required],
          lastName: ['', Validators.required],
          salary: ['', Validators.required],
          dateOfBirth: ['', Validators.required],
          dependents:[]
      });

      if (this.id) {
          this.loading = true;
          this.employeeService.getById(this.id)
              .pipe(first())
              .subscribe(x => {
                  this.form.patchValue(x.data);
                  this.form.get('dateOfBirth')?.setValue(this.getFormatedDate(new Date(x.data.dateOfBirth), 'YYYY-MM-dd'));
                  this.loading = false;
              });
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
      this.saveEmployee()
          .pipe(first())
          .subscribe({
              next: (response:any) => {
                if(response.success){
                  this.alertService.success('Employee saved', { keepAfterRouteChange: true });
                  this.router.navigateByUrl('/employee');
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

  private saveEmployee() {
      // create or update Employee based on id param
      return this.id
          ? this.employeeService.update(this.id!, this.form.value)
          : this.employeeService.create(this.form.value);
  }
}
