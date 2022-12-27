import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DependentRoutingModule } from './dependent-routing.module';
import { DependentComponent } from './dependent/dependent.component';
import { AddEditDependentComponent } from './add-edit-dependent/add-edit-dependent.component';
import { DependentListComponent } from './dependent-list/dependent-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AddDependentComponent } from './add-dependent/add-dependent.component';


@NgModule({
  declarations: [
    DependentComponent,
    AddEditDependentComponent,
    DependentListComponent,
    AddDependentComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DependentRoutingModule
  ]
})
export class DependentModule { }
