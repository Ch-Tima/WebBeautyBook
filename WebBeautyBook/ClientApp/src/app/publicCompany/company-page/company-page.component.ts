import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Company } from '../../models/Company';
import { Worker } from '../../models/Worker';
import { CompanyLike } from '../../models/CompanyLike';
import * as $ from 'jquery';
import { Service } from '../../models/Service';
import { MatDialog } from '@angular/material/dialog';
import { AppointmentDialogComponent } from '../appointment-dialog/appointment-dialog.component';

@Component({
  selector: 'app-company-page',
  templateUrl: './company-page.component.html',
  styleUrls: ['./company-page.component.css']
})
export class CompanyPageComponent {

  public company: Company = new Company;
  public workers: Worker[] = []
  public services: Service[] = []
  public searchText:any;

  private companyId:string|null;

  constructor(private http: HttpClient, public auth: AuthService, private activeRoute:ActivatedRoute, private rout: Router, private dialogRef : MatDialog){
    this.companyId = this.activeRoute.snapshot.queryParams['id'];

    if(this.companyId == null || this.companyId == undefined || this.companyId == ''){
      this.rout.navigate(["/"]);
      return;
    }
    this.loadCompany();
    this.loadWorkers();
  }

  public getOpenHours(dayOfWeek: number):string{
    var result = this.company?.companyOpenHours.find(x => x.dayOfWeek == dayOfWeek);
    if(result != undefined){
      return `${result.openFrom.substring(0, result.openFrom.lastIndexOf(":"))}-${result.openUntil.substring(0, result.openUntil.lastIndexOf(":"))}`;
    }else{
      return "Closed";
    }
  }

  public onClickLike(event: any){
    var status = $(event.srcElement).attr("onPressed")
    if(status == "false"){
      this.http.post(`api/CompanyLike?companyId=${this.company.id}`, "", {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
        this.company.isFavorite = true;
      }, e => {
        alert(e.error);
      })
    }else{
      this.http.delete(`api/CompanyLike?companyId=${this.company.id}`, {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
        this.company.isFavorite = false;
      }, e => {
        alert(e.error);
      })
    }
  }

  public booking(id:string){
    if(!this.auth.hasToken()){
      this.rout.navigate(["login"]);
      return;
    }
    const appointmentDialog = this.dialogRef.open(AppointmentDialogComponent, {
      width: "500px",
      data: id
    });
  }

  private async getAllMienLikes(){
    await this.http.get<CompanyLike[]>("api/CompanyLike", {
      headers: this.auth.getHeadersWithToken()
    }).toPromise().then(result => {
      var r = result?.findIndex(x => x.companyId == this.company.id)
      if(r != undefined && r != -1){
        this.company.isFavorite = true;
        console.log("ok");
      }
    }).catch(e => {
      console.log(e);
    });
  }

  private async loadCompany(){
    await this.http.get<Company>(`api/Company?id=${this.companyId}`).subscribe(
      async (result) => {
        this.company = result;
        if(this.auth.hasToken()) await this.getAllMienLikes();
      }, error => {
        console.log(error);
        this.rout.navigate(["/"]);
      }
    )
  }

  private async loadWorkers(){
    await this.http.get<Worker[]>(`api/Worker/getWorkersByCompanyId/${this.companyId}`).subscribe(
      result => {
        this.workers = result;
      }, error => {
        console.log(error);
        console.log("error -> loadWorkers");
      }
    )
  }

}