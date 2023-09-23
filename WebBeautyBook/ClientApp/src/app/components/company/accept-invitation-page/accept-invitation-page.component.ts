import { HttpClient } from '@angular/common/http';
import {Component, OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {AuthService} from "../../../services/auth/auth.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-accept-invitation-page',
  templateUrl: './accept-invitation-page.component.html',
  styleUrls: ['./accept-invitation-page.component.css']
})
export class AcceptInvitationPageComponent implements OnInit{

  public message: string = "";
  public status: -1|0|1 = 1;//-1 - error; 0 - load; 1 - Ok

  constructor(private auth: AuthService, private router: Router, private activityRoute: ActivatedRoute, private http: HttpClient, private toast: ToastrService) {
  }

  public ngOnInit() {
    const user = this.auth.getLocalUserDate();
    if(!user){
      this.router.navigate(["login"]);
      return;
    }
    //Pulling a token and an companyId from a URL
    const token = this.activityRoute.snapshot.queryParams["token"];
    const companyId = this.activityRoute.snapshot.queryParams["companyId"];
    //If token or companyId not found, redirect to home page
    if(!token || !companyId){
      this.router.navigate(["/"]);
    }else this.send(token, companyId);
  }

  private send(token:string, companyId:string){
    this.http.post(`api/Company/acceptInvitationToCompany?token=${token}&companyId=${companyId}`, {}, {
      headers: this.auth.getHeadersWithToken()
    }).toPromise()
      .then(r => {
        this.status = 1
        this.refreshTokens();
      })
      .catch(e => {
        this.message = e.error;
        this.status = -1
      });
  }

  //refreshTokens
  private refreshTokens(){
    this.auth.refreshTokens().toPromise()
      .then(result => {
        if (result == undefined){
          this.toast.info("refreshTokens is fail");
          this.auth.signOut();
          return;
        }else {
          this.auth.saveToken(result.token, result.expiration);
          window.location.replace("");
        }
      }).catch(e => {
      console.error(e);
      this.auth.signOut();
    })
  }

}
