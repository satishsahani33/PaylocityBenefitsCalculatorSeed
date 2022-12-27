import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DependentComponent } from './dependent/dependent.component';
import { AddEditDependentComponent } from './add-edit-dependent/add-edit-dependent.component';
import { DependentListComponent } from './dependent-list/dependent-list.component';
import { AddDependentComponent } from './add-dependent/add-dependent.component';

const routes: Routes = [
  {
    path: '', component: DependentComponent,
    children: [
      { path: '', component: DependentListComponent },
      { path: 'add', component: AddDependentComponent },
      { path: 'add/:id', component: AddDependentComponent },
      { path: 'edit/:id', component: AddEditDependentComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DependentRoutingModule { }
