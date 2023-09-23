import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import {Company} from "../../../models/Company";

@Component({
  selector: 'app-company-panel',
  templateUrl: './company-panel.component.html',
  styleUrls: ['./company-panel.component.css']
})
export class CompanyPanelComponent {

  companies: Company[] = [];
  
  constructor(private http: HttpClient){
    this.loadCompanies();
  }

  setCompanyList(data: Company){
    this.companies.push(data);
  }

  private loadCompanies(){
    this.http.get<Company[]>("api/Company/getAll").subscribe(
      result => {
        this.companies = result;
      }, error => {
        alert("ERROR");
        console.log(error);
      }
    );
  }

}
