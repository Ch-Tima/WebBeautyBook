import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  mForm: FormGroup;
  result: string;
  isErrorResult: boolean;

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private router: Router) { 
    if(auth.hasToken()) router.navigate(["/"]);
    this.mForm = formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email])
    });
    this.result = '';
    this.isErrorResult = false;
  }

  public onSubmit(){

    if(!this.mForm.valid) return;

    var email = this.mForm.controls["email"].getRawValue();
    this.auth.ForgotPassword(email)
      .subscribe(
        result => {
          console.log(result);
          this.result = 'We have sent you a password reset link to your email address "' + email + '"';
          this.isErrorResult = false;
        }, error => {
          console.log(error);
          this.result = error.error;
          this.isErrorResult = true;
        }
      );
  }
}
