import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent {
  isSuccess: boolean|null = null;
  errorMessage: string|null = null;

  constructor(private auth: AuthService, private router: Router, private activityRoute: ActivatedRoute) {
    //Pulling a token and an email address from a URL
    const token = this.activityRoute.snapshot.queryParams["token"];
    const email = this.activityRoute.snapshot.queryParams["email"];
    //If token or email address not found, redirect to home page
    if(!token || !email) this.router.navigate(["/"]);
    //Otherwise send token and email address to API
    this.send(token, email);
  }

  private send(token: string, email:string){
    this.auth.emailConfirmation(token, email).toPromise()
      .then(result => {
        this.isSuccess = true;
      }).catch(error => {
      this.isSuccess = false;
      this.errorMessage = error.error;
    });
  }
}
