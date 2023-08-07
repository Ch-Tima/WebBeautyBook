import { Component, ViewChild } from '@angular/core';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/Category';
import { Company } from '../models/Company';
import * as $ from "jquery";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {

  public topCompany: Company[] = []
  public categories: any[] = []
  @ViewChild('nav', {read: DragScrollComponent}) ds: DragScrollComponent | undefined;

  public constructor(private http: HttpClient){
    this.loadCopmanies();
    this.loadCategories();
  }

  //Carousel Recommended (top 12)
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

  public openCompanyProfile(id:string){
    console.log(id);
  }

  private loadCopmanies(){
    this.http.get<Company[]>("api/Company/getAll").subscribe(
      result => {
        this.topCompany = result;
        console.log(result);
      }, error => {
        console.log(error);
      }
    )
  }

  public onClickLike(id:string, event: any){
    console.log("send request or redirect");
    $(event.srcElement).prop("src", "../../assets/svg/heart-red.svg")
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
