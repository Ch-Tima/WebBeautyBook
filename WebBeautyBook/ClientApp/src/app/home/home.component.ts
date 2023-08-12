import { Component, ViewChild } from '@angular/core';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/Category';
import { Company } from '../models/Company';
import * as $ from "jquery";
import { AuthService } from '../services/auth.service';
import { CompanyLike } from '../models/CompanyLike';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {

  public topCompany: Company[] = []
  public categories: any[] = []
  @ViewChild('nav', {read: DragScrollComponent}) ds: DragScrollComponent | undefined;

  public constructor(private http: HttpClient, public auth: AuthService){
    this.loadCopmanies();
    this.loadCategories();
  }

  //Carousel Recommended (top 10)
 public moveNext() {
  if(this.ds == undefined){
    console.log("er");
    return;
  }
    this.ds.moveLeft();
  }

  public movePrevrol() {
    if(this.ds == undefined){
      return;
    }
    this.ds.moveRight();
  }

  public ngAfterViewInit() {
    setTimeout(() => {
      if(this.ds != undefined){
        this.ds.moveTo(3);
      }
    }, 0);
  }

  private loadCopmanies(){
    this.http.get<Company[]>("api/Company/getTopTen").subscribe(
      result => {
        this.topCompany = result;
        if(this.auth.hasToken()){
          //Load all my "CompanyLike" to find and install "red-heart.svg" in UI
          this.getAllMienLikes();
        }
      }, error => {
        console.log(error);
      }
    )
  }

  public onClickLike(id:string, event: any){
    var status = $(event.srcElement).attr("onPressed")
    if(status == "false"){
      this.http.post(`api/CompanyLike?companyId=${id}`, "", {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
        $(event.srcElement).prop("src", "../../assets/svg/heart-red.svg")
        $(event.srcElement).attr("onPressed", "true")
      }, e => {
        alert(e.error);
      })
    }else{
      this.http.delete(`api/CompanyLike?companyId=${id}`, {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(r => {
        $(event.srcElement).prop("src", "../../assets/svg/heart.svg")
        $(event.srcElement).attr("onPressed", "false")
      }, e => {
        alert(e.error);
      })
    }

  }

  private getAllMienLikes(){
    this.http.get<CompanyLike[]>("api/CompanyLike", {
      headers: this.auth.getHeadersWithToken()
    }).toPromise().then(result => {
      result?.forEach(item => {
        var heartImg = $(`#${item.companyId}`)
        heartImg.prop("src", "../../assets/svg/heart-red.svg")
        heartImg.attr("onPressed", "true")
      })
    }).catch(e => {
      console.log(e);
    });
  }

  //Enable Location Services
  public turnOnLocation(){
    alert("TODO")
    
  }

  public turnOffLocation(){
    $("#enable-location").css("display", "none")
  }

  //All categories
  private loadCategories(){
    this.http.get<Category[]>("api/Category/GetMainCategories", {}).subscribe(
      result => {
        this.categories = result;
      }, error => {
        console.log(error);
      }
    )
  }

}
