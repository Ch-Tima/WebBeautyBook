import {Component, EventEmitter, Input, Output, AfterViewInit} from '@angular/core';
import {LocationService} from "../../../services/location/location.service";

@Component({
  selector: 'app-search-company-input',
  templateUrl: './search-company-input.component.html',
  styleUrls: ['./search-company-input.component.css']
})
export class SearchCompanyInputComponent implements AfterViewInit {

  @Input()
  public companyName:string|undefined
  @Input()
  public categoryName:string|undefined
  @Input()
  public locationName:string|undefined
  @Input()
  public waitClick: boolean = true;
  @Output()
  public resultQuery = new EventEmitter<SearchData>();

  public constructor(private location: LocationService) {
  }

  // AfterViewInit lifecycle hook
  public ngAfterViewInit(): void {
    if(!this.waitClick){// Automatically trigger a query if waitClick is set to false
      this.getQuery();
    }
  }

  // Emits a search query with provided parameters
  public getQuery(){
    this.resultQuery.emit({
      companyName: this.companyName??'',
      categoryName: this.categoryName??'',
      locationName: this.locationName??''
    } as SearchData);
  }

}
// Interface to define the shape of a search query
export interface SearchData{
  companyName:string;
  categoryName:string;
  locationName:string;
}
