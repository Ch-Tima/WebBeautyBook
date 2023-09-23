import { HttpClient } from '@angular/common/http';
import { Component, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Company } from '../../../models/Company';
import { ToastrService } from 'ngx-toastr';
import { SearchData } from '../search-company-input/search-company-input.component';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.css']
})
export class SearchPageComponent {

  public name:string|undefined
  public category:string|undefined
  public location:string|undefined

  public companies:Company[]|undefined;
  public companyCardStyle:any;

  constructor(private activityRoute: ActivatedRoute, private http: HttpClient, private toast: ToastrService){
    // Retrieve query parameters from the route
    this.name = this.activityRoute.snapshot.queryParams["name"];
    this.category = this.activityRoute.snapshot.queryParams["category"];
    this.location = this.activityRoute.snapshot.queryParams["location"];
  }

  @HostListener('window:resize', ['$event'])
  private onResize(event:any) {// Adjust the company card style based on window width
    let innerWidth = window.innerWidth;
    if (innerWidth > 768) {
      this.companyCardStyle = 'big'
    }else{
      this.companyCardStyle = 'small'
    }
  }

  public async search(search:SearchData){
    this.companies = undefined;
    try {
      const response =  await this.http.get<Company[]>(`api/Company/Search?name=${search.companyName}&category=${search.categoryName}&location=${search.locationName}`).toPromise();
      // Check if the response is undefined or empty and handle it accordingly
      if (!response || response.length === 0) {
        this.companies = [];
      } else {
        this.companies = response;
      }
    }catch (e) {
      // Handle errors gracefully and display a toastr message
      this.toast.error("Error", "An error occurred. Please check the console for details.");
      console.error(e);
    }

  }

}
