import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {AuthService} from "../../../services/auth/auth.service";
import {ToastrService} from "ngx-toastr";// Import ToastrService for displaying toast messages

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {

  mForm: FormGroup; // Form group for login input fields
  lastErrorMessage: string = ""; // Variable to store the last error message

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private router: Router, private toast: ToastrService) {
    // Check if the user already has a token, if so, redirect to the home page
    if(auth.hasToken()) router.navigate(["/"]);
    this.mForm = formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required])
    });
  }

  public onSubmit() {
    // Check if the form is valid
    if (!this.mForm.valid) return;
    // Attempt to log in using the AuthService
    this.auth.login(this.mForm.value).toPromise()
      .then(result => {
        if(!result) {// Display a warning toast message if login fails
          this.toast.warning("Exit failed.");
        }else {
          this.mForm.reset();// Reset the form fields
          this.auth.saveToken(result.token, result.expiration);// Save the token and expiration date in the AuthService
          window.location.replace("");// Redirect to the home page
        }
      }).catch(error => {// Handle login error and store the error message
      this.lastErrorMessage = error.error;
      console.log(error)
    })
  }
}
