import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import {Company} from "../../../models/Company";

@Component({
  selector: 'app-company-panel',
  templateUrl: './company-panel.component.html',
  styleUrls: ['./company-panel.component.css']
})
export class CompanyPanelComponent {

  companies: Company[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string){
    this.loadCompanies();
  }

  setCompanyList(data: Company){
    this.companies.push(data);
  }

  private loadCompanies(){
    this.http.get<Company[]>(this.baseUrl + "api/Company/getAll").subscribe(
      result => {
        this.companies = result;
      }, error => {
        alert("ERROR");
        console.log(error);
      }
    );
  }

}
