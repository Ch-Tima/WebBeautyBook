import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {AuthService} from "../../../../services/auth/auth.service";
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { CompanyScheduleException } from 'src/app/models/CompanyScheduleException';

@Component({
  selector: 'app-schedule-exception-dialog',
  templateUrl: './schedule-exception-dialog.component.html',
  styleUrls: ['./schedule-exception-dialog.component.css']
})
export class ScheduleExceptionDialogComponent {

  public mForm:FormGroup;

  constructor(private auth: AuthService, private http: HttpClient, private formBuilder: FormBuilder, private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : any){
    //Override close for sending results
    this.dialog.backdropClick().subscribe(() => this.dialog.close(new ScheduleExceptionResult()))
    // Initialize the form with data or empty values based on update mode
    this.mForm = this.formBuilder.group({
      exceptionDate: [null, [Validators.required]],
      openFrom: ['00:00'],
      openUntil: ['00:00'],
      isClosed: [true],
      isOnce: [true],
      reason: ['', [Validators.maxLength(100)]],
    }, {
      validator: CompareValidator('openFrom', 'openUntil', "isClosed")
    });
  }

  public onChangedSlideToggle(event:MatSlideToggleChange){
    if(!event.checked){
      this.mForm.controls.openFrom.setValue('10:00');
      this.mForm.controls.openUntil.setValue('15:00');
    }else{
      this.mForm.controls.openFrom.setValue('00:00');
      this.mForm.controls.openUntil.setValue('00:00');
    }
  }

  public onSubmit(){
    this.http.put('/api/ScheduleException', this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(res => {
      this.dialog.close({
        status : 'add',
        value: res
      } as ScheduleExceptionResult)
    }, er => {
      console.log(er)
    })
  }

}

export class ScheduleExceptionResult{

  public status: "close"|"add"|"delete"|"none" = "none";
  public value: CompanyScheduleException|undefined;
}

export function CompareValidator(nameControlFirst: string, nameControlSecond: string, isClosed: string) {

  // Return a validator function that takes a FormGroup as input
  return (formGroup: FormGroup) => {
    // Get references to the two form controls we want to compare
    const control = formGroup.controls[nameControlFirst];
    const matchingControl = formGroup.controls[nameControlSecond];
    // Check if the second control already has an error of 'confirmedValidator'
    if (matchingControl.errors && !matchingControl.errors.confirmedValidator) return;
    // Compare the values of the two controls

    if (!formGroup.controls[isClosed].value && control.value === matchingControl.value) {
      matchingControl.setErrors({ confirmedValidator: false });
    } else {
      matchingControl.setErrors(null);
    }
  };
}
