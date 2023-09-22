import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../services/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Company } from '../../../models/Company';
import { Worker } from '../../../models/Worker';
import { CompanyLike } from '../../../models/CompanyLike';
import * as $ from 'jquery';
import { MatDialog } from '@angular/material/dialog';
import { AppointmentDialogComponent, AppointmentDialogDate, AppointmentDialogResult } from '../appointment-dialog/appointment-dialog.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-company-page',
  templateUrl: './company-page.component.html',
  styleUrls: ['./company-page.component.css']
})
export class CompanyPageComponent implements OnInit {

  public company: Company = new Company;
  public workers: Worker[]|undefined
  public searchText:any;
  private companyId:string|null;

  constructor(private toastr: ToastrService, private http: HttpClient, public auth: AuthService, private activeRoute:ActivatedRoute, private rout: Router, private dialogRef : MatDialog){
    this.companyId = this.activeRoute.snapshot.queryParams['id'];
    if(this.companyId == null || this.companyId == undefined || this.companyId == ''){
      this.rout.navigate(["/"]);
      return;
    }
  }

  public async ngOnInit() {
    await this.loadCompany();
    var workers = await this.loadWorkers();
    if(workers == undefined) this.workers = [];
    else this.workers = workers;
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
      width: "550px",
      data: {
        isUpdateMode: false,
        serviceId: id
      } as AppointmentDialogDate
    });
    appointmentDialog.afterClosed().subscribe((result:AppointmentDialogResult) => {
      if(result.isSuccess && result.action == 'create')
        this.toastr.success("Reservation was successful.")
    })
  }

  public copyToClipboard(val:string) {
    navigator.clipboard.writeText(val);
    this.toastr.success("Text copied!", undefined, { timeOut: 1000 })
  }

  private async getAllMienLikes(){
    return await this.http.get<CompanyLike[]>("api/CompanyLike", {
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
    return await this.http.get<Company>(`api/Company?id=${this.companyId}`).toPromise().then(async result => {
      if(result == undefined){
        this.toastr.error("Not found company!")
        this.rout.navigate(["/"]);
      }else{
        this.company = result;
        if(this.auth.hasToken()) await this.getAllMienLikes();
      }
    }).catch(e => {
      console.log(e);
      this.rout.navigate(["/"]);
    });
  }

  private async loadWorkers(){
    return await this.http.get<Worker[]>(`api/Worker/getWorkersByCompanyId/${this.companyId}`).toPromise();
  }

}
