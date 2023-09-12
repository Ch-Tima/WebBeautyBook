import { Component, EventEmitter, Input, Output, AfterViewInit } from '@angular/core';

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

  public ngAfterViewInit(): void {
    if(!this.waitClick){
      this.getQuery();
    }
  }

  public getQuery(){
    this.resultQuery.emit({
      companyName: this.companyName??'',
      categoryName: this.categoryName??'',
      locationName: this.locationName??''
    } as SearchData);
  }

}
export interface SearchData{
  companyName:string;
  categoryName:string;
  locationName:string;
}