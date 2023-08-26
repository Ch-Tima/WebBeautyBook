import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Reservation } from 'src/app/models/Reservation';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css'],
})
export class ReservationDialogComponent {

  public mForm:FormGroup;
  public error: string|undefined;

  constructor(private auth: AuthService, private http: HttpClient, private formBuilder: FormBuilder, @Inject('BASE_URL') private baseUrl: string, private dialg: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : ReservationDialogDate){
    //Override close for sending results
    this.dialg.backdropClick().subscribe(() => this.dialg.close(new ReservationDialogResult()))

    this.mForm = this.formBuilder.group({
      date: [data.isUpdateMode ? data.value?.date : null, [Validators.required]],
      timeStart: [data.isUpdateMode ? data.value?.timeStart : "09:00", [Validators.required]],
      timeEnd: [data.isUpdateMode ? data.value?.timeEnd : "10:00", [Validators.required]],
      description: [data.isUpdateMode ? data.value?.description : null, [Validators.maxLength(250)]],
    });

  }

  public onSubmit() {
    if(this.mForm.invalid) return;
    this.error = undefined;
    this.data.isUpdateMode ? this.update() : this.create();
  }

  public remove(){
    if(this.data.value?.id == undefined) {
      console.log('ReservationDialogComponent -> remove -> Not found id');
      return;
    }
    this.http.delete(`api/Reservation?id=${this.data.value?.id}`, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      this.dialg.close({
        isSuccess: true,
        action: 'remove',
        value: this.data.value
      } as ReservationDialogResult);
    }, error => this.showError(error))
  }

  private create(){
    this.http.put("api/Reservation", this.mForm.value, {
      headers: this.auth.getHeadersWithToken().append('Content-Type', 'application/json')
    }).subscribe(
      result => {
        this.dialg.close({
          isSuccess: true,
          action: 'create',
          value: result
        } as ReservationDialogResult);
      }, error => this.showError(error)
    );
  }

  private update(){

  }

  private showError(error:any){
    if(error.error.errors != undefined){ //erorr from model
      this.error = Object.values<any>(error.error.errors)[0][0];
    }else{ //error from controller
      this.error = error.error;
    }
  }

}
export class ReservationDialogDate{
  isUpdateMode: boolean = false;
  value: Reservation|null = null; 
}
export class ReservationDialogResult{
  public isSuccess: boolean = false;
  public action: 'remove'|'update'|'create'|'close' = 'close';
  public value: Reservation|null = null;
}