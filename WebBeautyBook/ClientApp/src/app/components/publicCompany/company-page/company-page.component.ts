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
import {finalize} from "rxjs";
import { TranslationService } from 'src/app/services/translation/translation.service';

@Component({
  selector: 'app-company-page',
  templateUrl: './company-page.component.html',
  styleUrls: ['./company-page.component.css']
})
export class CompanyPageComponent implements OnInit {

  public company: Company = new Company();
  public workers: Worker[] | undefined;
  public searchText:any;
  private companyId:string|null;
  private isRequestInProgress: boolean = false;// Flag to track whether an API request is in progress

  constructor(private toast: ToastrService, private http: HttpClient, public auth: AuthService, private activeRoute:ActivatedRoute, private rout: Router, private dialogRef : MatDialog, private translationService: TranslationService){
    // Retrieve company ID from route queryParams
    this.companyId = this.activeRoute.snapshot.queryParams['id'];
    // Redirect to the home page if company ID is missing or empty
    if(!this.companyId || this.companyId == ''){
      this.rout.navigate(["/"]);
      return;
    }
  }

  public async ngOnInit() {
    await this.loadCompany();// Load company and workers data on component initialization
    const workers = await this.loadWorkers();
    if(workers == undefined) this.workers = [];
    else this.workers = workers;// Set workers to an empty array if undefined
  }

  // Function to format open hours for a specific day
  public getOpenHours(dayOfWeek: number):string{
    const result = this.company?.companyOpenHours.find(x => x.dayOfWeek == dayOfWeek);
    if(result != undefined){
      return `${result.openFrom.substring(0, result.openFrom.lastIndexOf(":"))}-${result.openUntil.substring(0, result.openUntil.lastIndexOf(":"))}`;
    }else{
      return this.translationService.getTranslate("Closed");
    }
  }

  // Function to handle like/unlike button click
  public async onClickLike(event: any){
    // If a request is already in progress, do not allow further clicks
    if (this.isRequestInProgress) return;
    // Set the flag to true to indicate that a request is in progress
    else this.isRequestInProgress = true;
    const status = $(event.srcElement).attr("onPressed")
    const previousIsFavorite = this.company.isFavorite; // Store the previous value
    try {
      if(status == "false"){// Send a POST request to like the company
        await this.http.post(`api/CompanyLike?companyId=${this.company.id}`, "",{
            headers: this.auth.getHeadersWithToken()
          }).pipe(finalize(() => {
            this.isRequestInProgress = false;
          })).toPromise();
        this.company.isFavorite = true;
      }else{// Send a DELETE request to unlike the company
        await this.http.delete(`api/CompanyLike?companyId=${this.company.id}`,{
            headers: this.auth.getHeadersWithToken()
        }).pipe(finalize(() => {
          this.isRequestInProgress = false;
        })).toPromise();
        this.company.isFavorite = false;
      }
    }catch (error:any) {
      // Revert isFavorite to its previous value in case of an error
      this.company.isFavorite = previousIsFavorite;
      alert(error.error);
    }
  }

  // Function to handle booking button click
  public booking(id:string){
    if(!this.auth.hasToken()){
      this.rout.navigate(["login"]);// Redirect to the login page if the user is not authenticated
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
        this.toast.success("Reservation was successful.")
    })
  }

  // Function to copy text to clipboard and show a success toastr message
  public copyToClipboard(val:string) {
    navigator.clipboard.writeText(val);
    this.toast.success("Text copied!", undefined, { timeOut: 1000 })
  }

  // Function to check if the current user has liked the company
  private async getAllMienLikes(){
    return await this.http.get<CompanyLike[]>("api/CompanyLike", {
      headers: this.auth.getHeadersWithToken()
    }).toPromise().then(result => {
      const r = result?.findIndex(x => x.companyId == this.company.id)
      if(r != undefined && r != -1){
        this.company.isFavorite = true;
        console.log("ok");
      }
    }).catch(e => {
      console.error(e);
    });
  }

  // Function to load company data
  private async loadCompany(){
    return await this.http.get<Company>(`api/Company?id=${this.companyId}`).toPromise().then(async result => {
      if(result == undefined){
        this.toast.error("Not found company!")
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

  // Function to load workers data
  private async loadWorkers(){
    return await this.http.get<Worker[]>(`api/Worker/getWorkersByCompanyId/${this.companyId}`).toPromise();
  }

}
