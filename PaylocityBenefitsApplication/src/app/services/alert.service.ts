import { Injectable } from '@angular/core';
import { ConfirmationDialogComponent } from '@app/components/confirmation-dialog/confirmation-dialog.component';
import { PayCheckConfirmationDialogComponent } from '@app/components/pay-check/pay-check-confirmation-dialog/pay-check-confirmation-dialog.component';
import { Alert, AlertOptions, AlertType } from '@app/models';
import { ModalDismissReasons, NgbDatepickerModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
constructor(private modalService: NgbModal) { }

  private subject = new Subject<Alert>();
  private defaultId = 'default-alert';
  
  public confirm(
    title: string,
    message: string,
    btnOkText: string = 'OK',
    btnCancelText: string = 'Cancel',
    dialogSize: 'sm'|'lg' = 'sm'): Promise<boolean> {
    const modalRef = this.modalService.open(ConfirmationDialogComponent, { size: dialogSize });
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnOkText = btnOkText;
    modalRef.componentInstance.btnCancelText = btnCancelText;

    return modalRef.result;
  }

  public calculatePayCheckConfirmation(
    title: string,
    message: string,
    btnOkText: string = 'OK',
    btnCancelText: string = 'Cancel',
    dialogSize: 'sm'|'lg' = 'lg'): Promise<boolean> {
    const modalRef = this.modalService.open(PayCheckConfirmationDialogComponent, { size: dialogSize });
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnOkText = btnOkText;
    modalRef.componentInstance.btnCancelText = btnCancelText;

    return modalRef.result;
  }

  // enable subscribing to alerts observable
  onAlert(id = this.defaultId): Observable<Alert> {
      return this.subject.asObservable().pipe(filter(x => x && x.id === id));
  }

  // convenience methods
  success(message: string, options?: AlertOptions) {
      this.alert(new Alert({ ...options, type: AlertType.Success, message }));
  }

  error(message: string, options?: AlertOptions) {
      this.alert(new Alert({ ...options, type: AlertType.Error, message }));
  }

  info(message: string, options?: AlertOptions) {
      this.alert(new Alert({ ...options, type: AlertType.Info, message }));
  }

  warn(message: string, options?: AlertOptions) {
      this.alert(new Alert({ ...options, type: AlertType.Warning, message }));
  }

  // main alert method    
  alert(alert: Alert) {
      alert.id = alert.id || this.defaultId;
      this.subject.next(alert);
  }

  // clear alerts
  clear(id = this.defaultId) {
      this.subject.next(new Alert({ id }));
  }
}
