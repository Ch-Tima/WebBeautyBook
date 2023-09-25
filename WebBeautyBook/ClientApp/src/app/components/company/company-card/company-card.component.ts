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

  private wasClickOnLike:boolean = false;// Variable to prevent multiple clicks on the like button

  public constructor(private http: HttpClient, public auth: AuthService){}

  // Handle like button click
  public onClickLike(item:Company, event: any){
    // Check if the like button was clicked already to prevent multiple clicks
    if(this.wasClickOnLike)return;
    this.wasClickOnLike = true;
    const status = $(event.srcElement).attr("onPressed")
    // Get the current status of the "onPressed" attribute from the event source element
    if(status == "false"){// Like the company
      this.http.post(`api/CompanyLike?companyId=${item.id}`, "", {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(result => {
        item.isFavorite = true;// Update the "isFavorite" property of the company to indicate it's now liked
        this.clickLike.emit(item);// Emit the clickLike event with the liked company
      }, e => {// Display an error message on failure
        alert(e.error);
        console.error(e);
      },() => this.wasClickOnLike = false)
    }else{// Unlike the company
      this.http.delete(`api/CompanyLike?companyId=${item.id}`, {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
         item.isFavorite = false;// Update the "isFavorite" property of the company to indicate it's no longer liked
         this.clickLike.emit(item);// Emit the clickLike event with the unliked company
      }, e => {// Display an error message on failure
        alert(e.error);
        console.error(e);
      }, () => this.wasClickOnLike = false);
    }
  }

}
