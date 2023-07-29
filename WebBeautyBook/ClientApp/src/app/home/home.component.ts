import { Component, ViewChild } from '@angular/core';
import { DragScrollComponent } from 'ngx-drag-scroll';
import * as $ from "jquery";
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/Category';
import { error } from 'console';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {

  public topCompany: any[] = []
  public categories: any[] = []
  @ViewChild('nav', {read: DragScrollComponent}) ds: DragScrollComponent | undefined;

  public constructor(private http: HttpClient){
    for(var i = 0; i < 11; i++){
      this.topCompany.push(i)
    }
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
