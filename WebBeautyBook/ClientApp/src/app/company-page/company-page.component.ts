import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Company } from '../models/Company';
import { Worker } from '../models/Worker';

@Component({
  selector: 'app-company-page',
  templateUrl: './company-page.component.html',
  styleUrls: ['./company-page.component.css']
})
export class CompanyPageComponent {

  public company: Company = new Company;
  public workers: Worker[] = []
  public searchText:any;

  private companyId:string|null;

  constructor(private http: HttpClient, private auth: AuthService, private activeRoute:ActivatedRoute, private rout: Router){
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

  private loadCompany(){
    this.http.get<Company>(`api/Company?id=${this.companyId}`).subscribe(
      result => {
        this.company = result
      }, error => {
        console.log(error)
        this.rout.navigate(["/"])
      }
    )
  }

  private loadWorkers(){
    this.http.get<Worker[]>(`api/Worker/getWorkersByCompanyId/${this.companyId}`).subscribe(
      result => {
        this.workers = result;
      }, error => {
        console.log(error);
        console.log("error -> loadWorkers");
      }
    )
  }

}
