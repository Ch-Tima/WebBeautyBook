import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Location } from 'src/app/models/Location';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-location-form',
  templateUrl: './location-form.component.html',
  styleUrls: ['./location-form.component.css']
})
export class LocationFormComponent {

  public mForm: FormGroup;
  public header: string = "LocationFormComponent";
  public errorMessages : string = "";

  public location$ = this.loadCountries();

  constructor(private auth: AuthService, private http: HttpClient, private dialg: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : LocationFormDialogData){
     //Override close for sending results
    this.dialg.backdropClick().subscribe(() => {
      this.dialg.close(new LocationFormDialogData());
    });

    //Init form
    this.mForm = new FormGroup({
      id: new FormControl(data.value?.id??''),
      country: new FormControl(data.value?.country, [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
      city: new FormControl(data.value?.city, [Validators.minLength(3), Validators.maxLength(100)])
    });

    console.log(this.mForm.value)
  }

  public onSubmit(){
    if(this.mForm.valid){
      if(this.data.isUpdateMode){
        this.update();
      }else{
        this.insert();
      }
    }
  }

  private insert(){
    this.http.put<Location>("api/Location", this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        //preapation data
        var data = new LocationFormDialogResult();
        data.isSuccess = true;
        data.result = result
        this.dialg.close(data);//close dialog and send data
      }, error => {
        console.log(error);//show error
        this.errorMessages = error.error;
      }
    );
  }

  private update(){
    this.http.post(`api/Location?Id=${this.data.value?.id}`, this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      //preapation data
      var data = new LocationFormDialogResult();
      data.isSuccess = true;
      data.result = this.mForm.value
      this.dialg.close(data);//close dialog and send data
    }, error => {
      console.log(error);//show error
      this.errorMessages = error.error;
    });
  }

  public async loadCountries(){
    return await this.http.get<Location[]>("api/Location/getAllCountry").toPromise().catch(error => {
      console.log(error);
      return [];
    });
  }

}

export class LocationFormDialogData{
  isUpdateMode: boolean = true;
  value: Location|null = null;
}
export class LocationFormDialogResult{
  isSuccess: boolean = true
  result: Location|null = null
}
