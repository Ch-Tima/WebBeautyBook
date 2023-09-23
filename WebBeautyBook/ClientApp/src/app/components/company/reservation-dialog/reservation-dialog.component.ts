import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Reservation } from 'src/app/models/Reservation';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css'],
})
export class ReservationDialogComponent {

  // Declare class variables
  public mForm: FormGroup; // Form for reservation data
  public error: string|undefined; // Error message, if any

  constructor(private auth: AuthService, private http: HttpClient, private formBuilder: FormBuilder, private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : ReservationDialogDate){
    //Override close for sending results
    this.dialog.backdropClick().subscribe(() => this.dialog.close(new ReservationDialogResult()))
    // Initialize the form with data or empty values based on update mode
    this.mForm = this.formBuilder.group({
      date: [data.isUpdateMode ? data.value?.date : null, [Validators.required]],
      timeStart: [data.isUpdateMode ? data.value?.timeStart : "09:00", [Validators.required]],
      timeEnd: [data.isUpdateMode ? data.value?.timeEnd : "10:00", [Validators.required]],
      description: [data.isUpdateMode ? data.value?.description : null, [Validators.maxLength(250)]],
    });

  }

  // Handle form submission
  public onSubmit() {
    if(this.mForm.invalid) return;
    this.error = undefined;
    if(this.data.isUpdateMode ){
      this.update()
    }else{
      this.create();
    }
  }

  // Remove a reservation
  public remove(){
    if(this.data.value?.id == undefined) {
      console.log('ReservationDialogComponent -> remove -> Not found id');
      return;
    }
    this.http.delete(`api/Reservation?id=${this.data.value?.id}`, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      this.dialog.close({// Close the dialog with the result
        isSuccess: true,
        action: 'remove',
        value: this.data.value
      } as ReservationDialogResult);
    }, error => this.showError(error))
  }

  // Create a new reservation
  private create(){
    this.http.put("api/Reservation", this.mForm.value, {
      headers: this.auth.getHeadersWithToken().append('Content-Type', 'application/json')
    }).subscribe(
      result => {
        this.dialog.close({// Close the dialog with the result
          isSuccess: true,
          action: 'create',
          value: result
        } as ReservationDialogResult);
      }, error => this.showError(error)
    );
  }

  // Update an existing reservation
  private update(){
    this.http.post(`api/Reservation?id=${this.data.value?.id}`, this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {// Close the dialog with the result
      const v = this.mForm.value as Reservation;
      v.id = this.data.value?.id;
      v.workerId = this.data.value?.workerId;
      this.dialog.close({
        isSuccess: true,
        action: 'update',
        value: v
      } as ReservationDialogResult);
    }, error => this.showError(error));
  }

  // Handle and display error messages
  private showError(error:any){
    if(error.error.errors != undefined){ // Error from model
      this.error = Object.values<any>(error.error.errors)[0][0];
    }else{ // Error from controller
      this.error = error.error;
    }
  }

}
// Data class for the reservation dialog
export class ReservationDialogDate {
  isUpdateMode: boolean = false; // Flag indicating whether the dialog is in update mode
  value: Reservation|null = null; // Reservation data to be displayed in the dialog
}
// Result class for the reservation dialog
export class ReservationDialogResult {
  public isSuccess: boolean = false; // Flag indicating the success of the operation
  public action: 'remove'|'update'|'create'|'close' = 'close'; // Action performed in the dialog
  public value: Reservation|null = null; // Resulting reservation data
}
