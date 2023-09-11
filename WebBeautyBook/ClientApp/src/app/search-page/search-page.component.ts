import { HttpClient } from '@angular/common/http';
import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Company } from '../models/Company';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.css']
})
export class SearchPageComponent implements OnInit {

  public name:string|undefined
  public category:string|undefined
  public location:string|undefined

  public companies:Company[]|undefined;
  public companyCardStyle:any;

  constructor(private activityRoute: ActivatedRoute, private http: HttpClient, private toastr: ToastrService){
    this.name = this.activityRoute.snapshot.queryParams["name"];
    this.category = this.activityRoute.snapshot.queryParams["category"];
    this.location = this.activityRoute.snapshot.queryParams["location"];
  }

  public async ngOnInit() {
    await this.saerch();
  }

  @HostListener('window:resize', ['$event'])
  private onResize(event:any) {
    var innerWidth = window.innerWidth;
    if (innerWidth > 768) {
      this.companyCardStyle = 'big'
    }else{
      this.companyCardStyle = 'small'
    }
  }

  public async saerch(){
    var query = this.name != undefined && this.name.length > 0 ? `name=${this.name}`:''
    query += this.category != undefined && this.category.length > 0 ? `&category=${this.category}`:''
    query += this.location != undefined && this.location.length > 0 ? `&location=${this.location}`:''
    this.companies = undefined;
    await this.http.get<Company[]>(`api/Company/Search?${query}`).toPromise()
    .then(r => {
      if(r == undefined) this.companies = [];
      this.companies = r;
      console.log(this.companies);
    })
    .catch(e => {
      this.toastr.error("Error", "Look at the console.");
      console.error(e);
    });
  }

}
