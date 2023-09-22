import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import * as $ from "jquery";
import {AuthService, TOKEN} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-invitation-employee',
  templateUrl: './invitation-employee.component.html',
  styleUrls: ['./invitation-employee.component.css']
})
export class InvitationEmployeeComponent {

  public mForm: FormGroup;
  public message: string = "";

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private http: HttpClient) {
    this.mForm = formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email]),
    });
  }

  public onSubmit(){
    if(!this.mForm.valid){
      return;
    }
    this.http.post<string>("api/Company/InvitationToCompany", JSON.stringify(this.mForm.controls['email'].getRawValue()), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        "Authorization": "Bearer " + localStorage.getItem(TOKEN)
      })
    }).subscribe(
      result => {
        this.message = result;
        $('#message').css("color", "green");
      }, error => {
        console.log(error)
        this.message = error.error;
        $('#message').css("color", "red");
      }
    )

  }

}
