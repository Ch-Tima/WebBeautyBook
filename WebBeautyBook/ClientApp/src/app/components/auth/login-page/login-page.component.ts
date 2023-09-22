import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  mForm: FormGroup;
  lastErrorMessage: string = "";

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private router: Router) {
    if(auth.hasToken()) router.navigate(["/"]);
    this.mForm = formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required])
    });

  }

  onSubmit() {
    if (!this.mForm.valid) {
      console.log("Error!");
      console.log(this.mForm);
    } else {
      console.log("Send!");
      this.auth.login(this.mForm.value)
        .subscribe(resut => {
          this.mForm.reset();
          this.auth.saveToken(resut.token, resut.expiration);
          window.location.replace("");
        }, error => {
          this.lastErrorMessage = error.error;
          console.log(error)
        });
    }
  }
}
