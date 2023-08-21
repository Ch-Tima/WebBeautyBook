import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css']
})
export class ReservationDialogComponent {

  constructor(private auth: AuthService, private http: HttpClient, private formBuilder: FormBuilder, @Inject('BASE_URL') private baseUrl: string, private dialg: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : any){
    //Override close for sending results
    this.dialg.backdropClick().subscribe(() => {
      this.dialg.close(null);
    })
  }

}
