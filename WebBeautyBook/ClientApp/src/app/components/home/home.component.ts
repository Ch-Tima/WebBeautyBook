import {Component, OnInit, ViewChild} from '@angular/core';
import {DragScrollComponent} from 'ngx-drag-scroll';
import {HttpClient} from '@angular/common/http';
import {Category} from '../../models/Category';
import {Company} from '../../models/Company';
import * as $ from "jquery";
import {CompanyLike} from '../../models/CompanyLike';
import {Router} from '@angular/router';
import {SearchCompanyInputComponent, SearchData} from '../search/search-company-input/search-company-input.component';
import {LocationService} from "../../services/location/location.service";
import {ToastrService} from "ngx-toastr";
import {AuthService} from "../../services/auth/auth.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {

  public topCompany: Company[] | undefined = undefined;
  public categories: Category[] | undefined = undefined;
  @ViewChild('nav', {read: DragScrollComponent}) ds!: DragScrollComponent;
  @ViewChild('search', {read: SearchCompanyInputComponent}) search!: SearchCompanyInputComponent;

  public constructor(private toast: ToastrService, public location: LocationService, private http: HttpClient, public auth: AuthService, private router: Router) {}

  public async ngOnInit() {
    await this.loadCompanies();
    const categories = await this.loadCategories();
    if (categories != undefined) {
      this.categories = categories;
    } else this.categories = [];

    if (this.location.isLocationAccessGranted()) {
      await this.setLocationToSearch();
    }
  }

  // Opens a search and passes the results to the URL
  public openSearch(search: SearchData) {
    this.router.navigate([`/search`], {
      queryParams: {
        name: search.companyName,
        category: search.categoryName,
        location: search.locationName
      }
    });
  }

  // Carousel of recommended companies: next element
  public moveNext() {
    this.ds.moveLeft();
  }

  // Carousel of recommended companies: previous element
  public movePrevious() {
    this.ds.moveRight();
  }

  // Loads recommended companies
  private async loadCompanies() {
    return this.http.get<Company[]>("api/Company/getTopTen").toPromise()
      .then(result => {
        if (result == undefined || result.length == 0) {
          this.topCompany = [];
          return;
        } else {
          this.topCompany = result;
          if (this.auth.hasToken()) {//Load all my "CompanyLike" to find and install "red-heart.svg" in UI
            this.getAllMienLikes();
          }
        }
      }).catch(e => console.log(e));
  }

  private async getAllMienLikes() {
    await this.http.get<CompanyLike[]>("api/CompanyLike", {
      headers: this.auth.getHeadersWithToken()
    }).toPromise().then(result => {
      result?.forEach(item => {
        if (this.topCompany == undefined) return;
        //find and set isFavorite true
        const t = this.topCompany.find(x => x.id == item.companyId);
        if (t != undefined) t.isFavorite = true;
      })
    }).catch(e => console.log(e));
  }

  // Enable location service
  public async turnOnLocation() {
    $("#enable-location").css("display", "none");
    this.location.setLocationAccessGranted(true);
    await this.setLocationToSearch();
  }

  // Disable location service
  public async turnOffLocation() {
    $("#enable-location").css("display", "none")
    this.location.setLocationAccessGranted(false);
  }

  // Set location for search
  private async setLocationToSearch() {
    // Get the user's location from local storage
    const localLocation = this.location.getLocation();
    // Check if the local location is undefined or outdated
    if (localLocation == undefined || this.location.isLocationOld()) {
      try {
        // Request the user's location from the LocationService
        let location = await this.location.requestLocation();
        if (location == undefined) {// Check if the location request was successful
          this.toast.warning("requestLocation is fail")
          return;
        }
        this.search.locationName = location.city;// Update the locationName property of the search component
        this.location.setLocation(location);// Save the updated location to local storage
      } catch (e: any) {//HttpErrorResponse
        // Handle any errors that might occur during the location request
        console.error("Error requesting location:", e);
        if (e.status == 429) {
          this.location.setLocationAccessGranted(false);
          this.toast.info("Too many requests to 'Location'");
        }
      }
    } else {// Use the local location if it's available and up to date
      this.search.locationName = localLocation.city;
    }
  }

  // Load categories
  private async loadCategories(): Promise<void | Category[] | undefined> {
    try {
      return await this.http.get<Category[]>('api/Category/GetMainCategories', {}).toPromise();
    } catch (e) {
      console.log(e);
    }
  }

}
