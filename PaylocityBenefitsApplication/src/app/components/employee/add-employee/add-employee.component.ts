import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from '@app/models';
import { AlertService, EmployeeService } from '@app/services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent implements OnInit {
  form!: FormGroup;
  id?: string;
  title!: string;
  loading = false;
  submitting = false;
  submitted = false;
  showErrorMessage = false;
  errorMessage?:string;
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
          dateOfBirth: ['', [Validators.required, Validators.pattern(/^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$/)]],
          dependents: this.formBuilder.array([this.addDependentsGroup()])
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
  private addDependentsGroup(): FormGroup {
    return this.formBuilder.group({
      firstName: [],
      lastName: [],
      dateOfBirth: ['', [Validators.required, Validators.pattern(/^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$/)]],
      relationship: ['', Validators.required],
    });
  }
  addDependent(): void {
    this.dependentsArray.push(this.addDependentsGroup());
    console.log(this.dependentsArray);
  }
  removeDependent(index: number): void {
    this.dependentsArray.removeAt(index);
  }

  get dependentsArray(): FormArray {
    return <FormArray>this.form.get('dependents');
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
                this.submitting = false;
                if(response.success){
                  this.alertService.success('Employee saved', { keepAfterRouteChange: true });
                  this.router.navigateByUrl('/employee');}
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
    let employee = this.form.value;
    let i: number;
    for(i=0;i<employee.dependents.length;i++){
        employee.dependents[i].relationship = Number(employee.dependents[i].relationship);
    }
    return this.employeeService.create(employee);
  }
}