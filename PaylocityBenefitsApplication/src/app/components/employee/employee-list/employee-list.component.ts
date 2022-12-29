import { Component, OnInit } from '@angular/core';
import { AlertService, EmployeeService } from '@app/services';
import { first } from 'rxjs/operators';
import { ModalDismissReasons, NgbDatepickerModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees?: any[];
  payCheckForm!: FormGroup;
  employeeId: any;
  submitted = false;
  payCheckTitle?: string;
  years?: any[];
  payCheckNumbers?: any[];
	page = 1;
  pageSize = 10;
  totalEmployees: number = 0;
  
  pageNumber?: number;
  employeePageSize?: number;
  orderBy?: string;
  sortBy?:string
  
  constructor(
    private formBuilder: FormBuilder,private employeeService: EmployeeService,
    private alertService: AlertService, private modalService: NgbModal,
    private router: Router) {
      this.pageNumber = 1;
      this.employeePageSize = 15;
      this.orderBy = 'Id';
      this.sortBy = 'desc';
      this.employees = [];
    }
    
    openPayCheckConfirmation(content:any, id:string, title: string) {
      console.log(title + ' Pay Check Started for employee with id = '+ id);
      this.payCheckTitle = title;
      this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
        (result) => {
          if(result){
             // reset alerts on submit
            this.alertService.clear();

            // stop here if form is invalid
            if (this.payCheckForm.invalid) {
              this.alertService.error('Please select both year and month.');
                return;
            }
            if(title == 'View Only'){
              this.router.navigateByUrl('/employee/viewPayCheck/'+ id +'/'+this.payCheckForm.value.payCheckYear + '/'+this.payCheckForm.value.payCheckMonth);
              console.log('Pay check will be viewed for employee id ' + id + ' of year = ' + 
              this.payCheckForm.value.payCheckYear + ' and month = ' + this.payCheckForm.value.payCheckMonth);
            }
            else{
              this.router.navigateByUrl('/employee/calculatePayCheck/'+ id +'/'+this.payCheckForm.value.payCheckYear + '/'+this.payCheckForm.value.payCheckMonth);
              console.log('Pay check will be calculated for employee id ' + id + ' of year = ' + 
              this.payCheckForm.value.payCheckYear + ' and month = ' + this.payCheckForm.value.payCheckMonth);
            }
            
          }
        }
      );
    }
  ngOnInit() {
      this.payCheckForm = this.formBuilder.group({
          payCheckYear: ['', Validators.required],
          payCheckMonth:['', Validators.required]
      });
      this.years = [
        {id: '', name: 'Please Select'},
        {id: '2019', name: '2019'},
        {id: '2020', name: '2020'},
        {id: '2021', name: '2021'},
        {id: '2022', name: '2022'}
      ];
      this.payCheckNumbers = [
        {id: '', name: 'Please Select'},
        {id: 'Pay Check 1', name: 'Pay Check 1'},
        {id: 'Pay Check 2', name: 'Pay Check 2'},
        {id: 'Pay Check 3', name: 'Pay Check 3'},
        {id: 'Pay Check 4', name: 'Pay Check 4'},
        {id: 'Pay Check 5', name: 'Pay Check 5'},
        {id: 'Pay Check 6', name: 'Pay Check 6'},
        {id: 'Pay Check 7', name: 'Pay Check 7'},
        {id: 'Pay Check 8', name: 'Pay Check 8'},
        {id: 'Pay Check 9', name: 'Pay Check 9'},
        {id: 'Pay Check 10', name: 'Pay Check 10'},
        {id: 'Pay Check 11', name: 'Pay Check 11'},
        {id: 'Pay Check 12', name: 'Pay Check 12'},
        {id: 'Pay Check 13', name: 'Pay Check 13'},
        {id: 'Pay Check 14', name: 'Pay Check 14'},
        {id: 'Pay Check 15', name: 'Pay Check 15'},
        {id: 'Pay Check 16', name: 'Pay Check 16'},
        {id: 'Pay Check 17', name: 'Pay Check 17'},
        {id: 'Pay Check 18', name: 'Pay Check 18'},
        {id: 'Pay Check 19', name: 'Pay Check 19'},
        {id: 'Pay Check 20', name: 'Pay Check 20'},
        {id: 'Pay Check 21', name: 'Pay Check 21'},
        {id: 'Pay Check 22', name: 'Pay Check 22'},
        {id: 'Pay Check 23', name: 'Pay Check 23'},
        {id: 'Pay Check 24', name: 'Pay Check 24'},
        {id: 'Pay Check 25', name: 'Pay Check 25'},
        {id: 'Pay Check 26', name: 'Pay Check 26'},
      ];
      this.fetchEmployees();
  }

  fetchEmployees(){
    this.page = 1; //when next block of emplyee is fetched, initialize to page 1
    this.employeeService.getAll(this.pageNumber, this.employeePageSize, this.orderBy,this.sortBy)
    .pipe(first())
    .subscribe(employees =>
      { 
        if(employees.success && employees.data.length > 0){
          this.employees = employees.data;
          this.totalEmployees= Number(this.employees?.length);
          console.log('Current Employe block = ' + this.pageNumber);
        }
        else{
          this.alertService.info('No employee available, Please add employee.');
          this.alertService.info('Please click Initialize Data to load employees.');
        }
      });
  }
  deleteEmployee(id: string) {
      const employee = this.employees!.find(x => x.id === id);
      employee.isDeleting = true;
      this.employeeService.delete(id)
          .pipe(first())
          .subscribe(() => {this.employees = this.employees!.filter(x => x.id !== id);
            this.alertService.success('Employee deleted', { keepAfterRouteChange: true });});
  }
  public openConfirmationDialog(id: string) {
    this.alertService.confirm('Please confirm', 'Are you sure you want to delete?')
    .then((confirmed) => 
    {
      if(confirmed){
        console.log('Employee deleted, id= :', id);
        this.deleteEmployee(id);
      }else{
        console.log('User dismissed the dialog');
      }
    })
    .catch(() => console.log('User dismissed the dialog'));
  }
  currentBlock: number = 1
  fetchNextBlock(){
    if(this.pageNumber){
      if(this.employees && this.employeePageSize)
      if( this.employees.length == this.employeePageSize){
        this.pageNumber++;
        this.fetchEmployees();
      }
    }
  }
  fetchPreviousBlock(){
    if(this.pageNumber){
      if(this.pageNumber - 1 == 0){
        this.pageNumber = 1;
      }else{
        this.pageNumber--;
        this.fetchEmployees();
      }
    }
    
  }
}
