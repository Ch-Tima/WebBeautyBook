import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {AuthService} from "../../../services/auth/auth.service";
import { CompareValidator } from 'src/app/validators/CompareValidator';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  mForm: FormGroup = new FormGroup({});
  lastErrorMessage: string = "";

  constructor(private auth: AuthService, private formBuilder: FormBuilder,
    private router: Router, private activityRoute: ActivatedRoute)
  {
    this.mForm = formBuilder.group({
      email: new FormControl('', [Validators.required]),
      token: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)]),
      confirmPassword: new FormControl('', [Validators.required]),
    }, {
      validator: CompareValidator("password", "confirmPassword")
    });
  }

  ngOnInit(): void {
    //Pulling a token and an email address from a URL
    var token = this.activityRoute.snapshot.queryParams["token"];
    var email = this.activityRoute.snapshot.queryParams["email"];

    //If token or email address not found, redirect to home page
    if(token == undefined || email == undefined) this.router.navigate(["/"]);

    //Add a token and an email to the form
    this.mForm.controls['token'].setValue(token);
    this.mForm.controls['email'].setValue(email);
  }

  onSubmit() {
    this.auth.ResetPassword(this.mForm.value)
      .subscribe(result => {
        this.router.navigate(["/login"]);
      }, error => {
        //If there is an error in the model, then a list of errors will come
        if(error.error.errors != undefined && Object.values(error.error.errors)[0] != undefined){
          this.lastErrorMessage = Object.values(error.error.errors)[0] + "";
        }else{//Just an error line
          this.lastErrorMessage = error.error;
        }
      });
  }
}
