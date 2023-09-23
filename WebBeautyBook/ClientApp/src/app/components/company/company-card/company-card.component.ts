import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Company } from 'src/app/models/Company';
import * as $ from 'jquery';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-company-card',
  templateUrl: './company-card.component.html',
  styleUrls: ['./company-card.component.css']
})
export class CompanyCardComponent {

  @Input()
  public company: Company | undefined = undefined; // Input property for company data
  @Input()
  public style: 'small' | 'big' = 'small'; // Input property for card style
  @Output()
  public clickLike: EventEmitter<Company> = new EventEmitter(); // Output event emitter for liking a company

  public constructor(private http: HttpClient, public auth: AuthService){}

  // Handle like button click
  public onClickLike(item:Company, event: any){
    const status = $(event.srcElement).attr("onPressed")
    if(status == "false"){// Like the company
      this.http.post(`api/CompanyLike?companyId=${item.id}`, "", {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
        item.isFavorite = true;
        this.clickLike.emit(item);// Emit the clickLike event with the liked company
      }, e => {
        alert(e.error);// Display an error message on failure
      })
    }else{// Unlike the company
      this.http.delete(`api/CompanyLike?companyId=${item.id}`, {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
         item.isFavorite = false;
         this.clickLike.emit(item);// Emit the clickLike event with the unliked company
      }, e => {
        alert(e.error);// Display an error message on failure
      })
    }
  }

}
