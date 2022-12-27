import { Component, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from '@app/services';
import { ModalDismissReasons, NgbActiveModal, NgbDatepickerModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-pay-check-confirmation-dialog',
  templateUrl: './pay-check-confirmation-dialog.component.html',
  styleUrls: ['./pay-check-confirmation-dialog.component.css']
})
export class PayCheckConfirmationDialogComponent {
  
  @Input() title: string;
  @Input() message: string;
  @Input() btnOkText: string;
  @Input() btnCancelText: string;

  payCheckForm!: FormGroup;
  years?: any[];
  payCheckNumbers?: any[];
  submitted = false;
  payCheckTitle?: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private alertService: AlertService,
    private activeModal: NgbActiveModal) {
        this.title='';
        this.message='';
        this.btnOkText='';
        this.btnCancelText='';
    }
  ngOnInit(){   
    

    this.payCheckTitle = 'Calculate';
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
    
  }
  public decline() {
    this.activeModal.close(false);
  }

  public accept() {
    this.activeModal.close(this.payCheckForm.value);
  }

  public dismiss() {
    this.activeModal.dismiss();
  }
}
