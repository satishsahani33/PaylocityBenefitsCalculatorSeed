import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService, DependentService } from '@app/services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-add-edit-dependent',
  templateUrl: './add-edit-dependent.component.html',
  styleUrls: ['./add-edit-dependent.component.css']
})
export class AddEditDependentComponent implements OnInit {
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
      private dependentService: DependentService,
      private alertService: AlertService
  ) {}

  ngOnInit() {
      this.id = this.route.snapshot.params['id'];
      
      this.form = this.formBuilder.group({
          firstName: ['', Validators.required],
          lastName: ['', Validators.required],
          relationship: ['', Validators.required],
          dateOfBirth: ['', [Validators.required, Validators.pattern(/^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$/)]],
      });

      this.title = 'Add Dependent';
      if (this.id) {
          // edit mode
          this.title = 'Edit Dependent';
          this.loading = true;
          this.dependentService.getById(this.id)
              .pipe(first())
              .subscribe(x => {
                if(x.data){
                  this.form.patchValue(x.data);
                  this.form.get('dateOfBirth')?.setValue(this.getFormatedDate(new Date(x.data.dateOfBirth), 'YYYY-MM-dd'));
                  this.loading = false;
                }
                else{
                    if(x.message)
                        this.alertService.error(x.message);
                }
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
      this.saveDependent()
          .pipe(first())
          .subscribe( {
              next: (response:any) => {
                if(response.success){
                  this.alertService.success('Dependent saved', { keepAfterRouteChange: true });
                  this.router.navigateByUrl('/employee/dependents/' + response.data.id);
                }else{
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
      return this.id
          ? this.dependentService.update(this.id!, this.form.value)
          : this.dependentService.create(this.form.value);
  }
}
