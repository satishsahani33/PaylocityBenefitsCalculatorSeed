import { Component } from '@angular/core';
import { AlertService, EmployeeService } from './services';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'PaylocityBenefitsApplication';
  employees? : any[];
  constructor(private employeeService: EmployeeService,private alertService: AlertService
    ,private router: Router){}

  public openConfirmationDialog() {
    this.alertService.confirm('Please Confirm', 'This opeartion will load mocked data into database.')
    .then((confirmed) => 
    {
      if(confirmed){
        console.log('Employee loading...');
        this.loadSeedData();
      }else{
        console.log('User dismissed the dialog');
      }
    })
    .catch(() => console.log('User dismissed the dialog'));
  }

  public loadSeedData() {
    this.employeeService.createMockData('LoadMockData')
    .pipe(first())
    .subscribe(x => 
      {
        if(x.success)
        {
          console.log(x.data);
          this.alertService.success('Employees added successfully', { keepAfterRouteChange: true });
          this.router.navigateByUrl('/employee');
        } else{
          this.alertService.error('Employee can not be added');
        }
      }
    );


  }
}
