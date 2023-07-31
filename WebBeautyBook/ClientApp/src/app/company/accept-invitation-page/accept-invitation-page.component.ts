import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { er } from '@fullcalendar/core/internal-common';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-accept-invitation-page',
  templateUrl: './accept-invitation-page.component.html',
  styleUrls: ['./accept-invitation-page.component.css']
})
export class AcceptInvitationPageComponent {

  public message: string = "";
  //-1 - error; 0 - load; 1 - Ok
  public status: -1|0|1 = 1;

  constructor(private auth: AuthService, private router: Router, private activityRoute: ActivatedRoute, private http: HttpClient) { 

    var user = this.auth.getLocalUserDate();
    if(user == null){
      this.router.navigate(["login"]);
    }

    //Pulling a token and an companyId from a URL
    const token = this.activityRoute.snapshot.queryParams["token"];
    const companyId = this.activityRoute.snapshot.queryParams["companyId"];
    
    //If token or companyId not found, redirect to home page
    if(token == undefined || companyId == undefined) this.router.navigate(["/"]);

    console.log("token:" + token);
    console.log("companyId:" + companyId);
    this.send(token, companyId);
  }

  private send(token:string, companyId:string){
    this.http.post(`api/Company/acceptInvitationToCompany?token=${token}&companyId=${companyId}`, {}, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        console.log(result);
        this.status = 1
        this.refreshTokens();
      }, error=> {
        console.log(error);
        this.message = error.error;
        this.status = -1
      }
    );
  }

  //refreshTokens
  private refreshTokens(){
    this.auth.refreshTokens().subscribe(
      result => {
        this.auth.saveToken(result.token, result.expiration);
        window.location.replace("");
      }, error => {
        console.log(error);
        this.auth.signOut();
      }
    )
  }

}
