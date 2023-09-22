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
  public company: Company|undefined = undefined;
  @Input()
  public style:'small'|'big' = 'small';
  @Output()
  public clickLike: EventEmitter<Company> = new EventEmitter();

  public constructor(private http: HttpClient, public auth: AuthService){

  }

  public onClickLike(item:Company, event: any){
    var status = $(event.srcElement).attr("onPressed")
    if(status == "false"){
      this.http.post(`api/CompanyLike?companyId=${item.id}`, "", {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
        item.isFavorite = true;
        this.clickLike.emit(item);
      }, e => {
        alert(e.error);
      })
    }else{
      this.http.delete(`api/CompanyLike?companyId=${item.id}`, {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
         item.isFavorite = false;
         this.clickLike.emit(item);
      }, e => {
        alert(e.error);
      })
    }
  }

}
