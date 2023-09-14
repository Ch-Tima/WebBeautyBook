import { Component, OnInit, ViewChild } from '@angular/core';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/Category';
import { Company } from '../models/Company';
import * as $ from "jquery";
import { AuthService } from '../services/auth.service';
import { CompanyLike } from '../models/CompanyLike';
import { Router } from '@angular/router';
import { SearchData } from '../search-company-input/search-company-input.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {

  public topCompany: Company[]|undefined = undefined;
  public categories: Category[]|undefined = undefined;
  @ViewChild('nav', {read: DragScrollComponent}) ds: DragScrollComponent | undefined;

  public constructor(private http: HttpClient, public auth: AuthService, private router: Router){
  }
  
  public async ngOnInit() {
    await this.loadCopmanies();
    var categuries = await this.loadCategories();
    if(categuries != undefined){
      this.categories = categuries;
    }else this.categories = [];
  }

  public openSearch(search:SearchData){
    console.log(search);
    this.router.navigate([`/search`], { queryParams: { name: search.companyName, category: search.categoryName, location: search.locationName} });
  }

  //Carousel Recommended (top 10)
  public moveNext() {
    if(this.ds == undefined) return;
    this.ds.moveLeft();
  }

  public movePrevrol() {
    if(this.ds == undefined){
      return;
    }
    this.ds.moveRight();
  }

  private async loadCopmanies(){
    return this.http.get<Company[]>("api/Company/getTopTen").toPromise()
    .then(result => {
      if(result == undefined || result.length == 0) {
        this.topCompany = [];
        return;
      }else{
        this.topCompany = result;
        if(this.auth.hasToken()){//Load all my "CompanyLike" to find and install "red-heart.svg" in UI
          this.getAllMienLikes();
        }
      }
    }).catch(e => console.log(e));
  }

  private async getAllMienLikes(){
    await this.http.get<CompanyLike[]>("api/CompanyLike", {
      headers: this.auth.getHeadersWithToken()
    }).toPromise().then(result => {
      result?.forEach(item => {
        if(this.topCompany == undefined) return;
        //find and set isFavorite true
        var t = this.topCompany.find(x => x.id == item.companyId);
        if(t != undefined) t.isFavorite = true;
      })
    }).catch(e => console.log(e));
  }

  //Enable Location Services
  public turnOnLocation(){
    $("#enable-location").css("display", "none")
  }

  public turnOffLocation(){
    $("#enable-location").css("display", "none")
  }

  //All categories
  private async loadCategories():Promise<void|Category[]|undefined>{
    return await this.http.get<Category[]>("api/Category/GetMainCategories", {}).toPromise().catch(e => console.log(e));
  }

}
