import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Relationship } from '@app/models';
import { AlertService, DependentService } from '@app/services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-dependent-list',
  templateUrl: './dependent-list.component.html',
  styleUrls: ['./dependent-list.component.css']
})
export class DependentListComponent implements OnInit {
  dependents?: any[];
  id?: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private dependentService: DependentService,
    private alertService: AlertService
    ) {}
  
  ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    if (this.id) {
      this.dependentService.getAllDependentsOfEmployee(this.id)
          .pipe(first())
          .subscribe(dependents => 
            {
              if(dependents.success)
                this.dependents = dependents.data;
              else{
                this.alertService.info('No dependents available, Please add employee.');
              }
            });
    }else{
      this.dependentService.getAll()
          .pipe(first())
          .subscribe(dependents => 
            {
              if(dependents.success && dependents.data.length > 0)
                this.dependents = dependents.data;
              else{
                this.alertService.info('No dependents available, Please add employee.');
              }
            });
    }
  }
  mapRelationship(id: number){
    if(Relationship.None == id)
      return "None";
    else if(Relationship.Spouse == id)
      return "Spouse";
    else if(Relationship.DomesticPartner == id)
      return "DomesticPartner";
    else if(Relationship.Child == id)
      return "Child";
    else
      return "";
  }
  deleteDependent(id: string) {
      const dependent = this.dependents!.find(x => x.id === id);
      dependent.isDeleting = true;
      this.dependentService.delete(id)
          .pipe(first())
          .subscribe(() => {
            this.dependents = this.dependents!.filter(x => x.id !== id);
            this.alertService.success('Dependent deleted', { keepAfterRouteChange: true });
          });
  }
  addDependent(){
    console.log('employee id = '+this.id);
  }
  
  public openConfirmationDialog(id: string) {
    this.alertService.confirm('Please confirm', 'Are you sure you want to delete?')
    .then((confirmed) => 
    {
      if(confirmed){
        console.log('Dependent deleted, id= :', id);
        this.deleteDependent(id);
      }else{
        console.log('User dismissed the dialog');
      }
    })
    .catch(() => console.log('User dismissed the dialog'));
  }
}