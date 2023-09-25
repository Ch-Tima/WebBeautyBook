import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {

  mForm: FormGroup;// Form group for forgot password input fields
  result: string = '';// Variable to store the result message
  isErrorResult: boolean = false; // Boolean to indicate if the result is an error

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private router: Router) {
    // Check if the user already has a token, if so, redirect to the home page
    if(auth.hasToken()) router.navigate(["/"]);
    // Initialize the forgot password form with validation
    this.mForm = formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  public onSubmit(){
    if(!this.mForm.valid) return;
    const email = this.mForm.controls["email"].getRawValue();
    this.auth.ForgotPassword(email).toPromise().then(result => {
      this.result = 'We have sent you a password reset link to your email address "' + email + '"';
      this.isErrorResult = false;
    }).catch(error => {
      console.error(error);
      this.result = error.error;
      this.isErrorResult = true;
    })
  }
}
