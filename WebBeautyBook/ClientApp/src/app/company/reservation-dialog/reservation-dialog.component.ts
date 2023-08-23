import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css'],
})
export class ReservationDialogComponent {

  public mForm:FormGroup;
  public error: string|undefined;

  constructor(private auth: AuthService, private http: HttpClient, private formBuilder: FormBuilder, @Inject('BASE_URL') private baseUrl: string, private dialg: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : any){
    //Override close for sending results
    this.dialg.backdropClick().subscribe(() => {
      this.dialg.close(null);
    })

    this.mForm = this.formBuilder.group({
      date: [null, [Validators.required]],
      timeStart: ["09:00", [Validators.required]],
      timeEnd: ["10:00", [Validators.required]],
      description: [null, [Validators.maxLength(250)]],
    });
  }

  public onSubmit() {
    if(this.mForm.invalid) return;

    this.error = undefined;

    this.http.put("api/Reservation", this.mForm.value, {
      headers: this.auth.getHeadersWithToken().append('Content-Type', 'application/json')
    }).subscribe(
      result => {
        this.dialg.close(result);
      }, error => {
        if(error.error.errors != undefined){ //erorr from model
          this.error = Object.values<any>(error.error.errors)[0][0];
        }else{ //error from controller
          this.error = error.error;
        }
      }
    );
  }
}
