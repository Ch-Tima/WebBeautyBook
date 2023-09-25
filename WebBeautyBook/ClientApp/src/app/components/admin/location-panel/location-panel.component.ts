import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Location } from 'src/app/models/Location';
import { LocationFormComponent, LocationFormDialogData, LocationFormDialogResult } from '../location-form/location-form.component';
import * as $ from "jquery";

@Component({
  selector: 'app-location-panel',
  templateUrl: './location-panel.component.html',
  styleUrls: ['./location-panel.component.css']
})
export class LocationPanelComponent {

  public locations$ = this.loadLocationAsync();

  public constructor(private http: HttpClient, private dialog: MatDialog){
  }

  // Open the location creation form dialog
  public openFormCreateLocation(){
    //prepare data for the dialog
    const data = new LocationFormDialogData()
    data.isUpdateMode = false;
    //open dialog
    const dialog = this.dialog.open(LocationFormComponent, {
      data: data,
      width: '400px',
    });
    //subscribe to afterClosed
    dialog.afterClosed().subscribe((result:LocationFormDialogResult) => {
      if(!result.isSuccess) return;
       this.locations$.then(arr => {
        const item = result.result;
          if(arr == undefined || item == null) return;
          let findCountry = arr.find(x => x.country == item?.country)
            if(findCountry == undefined){//create new and push
              arr.push(new Country(item.country, [item]))
            }else{//insert a new city into an existing country
              findCountry.cities.push(item);
            }
       })
    });
  }

  // Open the location update form dialog
  public openFormUpdateLocation(index:number) {
    console.log(index);
    const item = this.locations$.then(arr => {
      const item = arr != undefined ? arr[index]: undefined;
      if(item == undefined) return;
      const val = $(`#${item.country}`).val();
      return item.cities.find(x => x.city == val);
    });

    if(item == undefined) return;

    item.then(x => {
      //prepare data for the dialog
      const data = new LocationFormDialogData()
      data.isUpdateMode = true;
      data.value = x??null;
      const dialog = this.dialog.open(LocationFormComponent, {
        data: data,
        width: '400px',
      });
      //subscribe to afterClosed
      dialog.afterClosed().subscribe((value:LocationFormDialogResult) => {
        if(!value.isSuccess) return;
        this.locations$.then(arr => {
          if(arr == undefined || value.result == null) return;
          let country = arr.find(x => x.cities.find(z => z.id == value.result?.id))
          if(!country) return;

          if(value.result.country == country.country){// if the country is not changed, just update the data
            //just update date
            let item = country.cities.find(x => x.id == value.result?.id)
            if(item == undefined) return;
            item.city = value.result.city;
            return;//end
          }
          //else
          if(country.cities.length > 1)//if this is not last item
              country.cities.splice(country.cities.findIndex(x => x.id == value.result?.id), 1)
            else //else deletes country from list
              arr.splice(arr.indexOf(country), 1);

            let newCountry = arr.find(z => z.country == value.result?.country)
            //push to exist country
            if(newCountry != undefined)newCountry.cities.push(value.result);
            //push new
            else arr.push(new Country(value.result.country, [value.result]))

        })
      });

    });
  }

  public delete(){
    alert("TODO");
  }

  // Load locations asynchronously
  private async loadLocationAsync() {
    return await this.http.get<Location[]>("api/Location/getAll").toPromise()
    .catch(error => {
      console.log(error)
      console.log("LocationPanelComponent -> loadLocation")
    }).then(x => {
      if(x == undefined){
        console.log("error -> loadLocationAsync -> then")
        return
      }
      //grouping by country
      let filterResult = x.reduce((group:any, item:any) => {
        const { country } = item
        group[country] = group[country] ?? []
        group[country].push(item)
        return group
      }, {});
      //Convert to regular object
      let list:Country[] = []
      Object.keys(filterResult).forEach(async item => {
        list.push(new Country(item, filterResult[item]))
      });
      return list;
    })
  }

}
class Country{
  public country: string
  public cities: Location[]
  constructor(country:string, cities:Location[]){
    this.country = country;
    this.cities = cities
  }
}
