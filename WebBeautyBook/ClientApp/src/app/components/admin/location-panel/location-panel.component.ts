import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Location } from 'src/app/models/Location';
import { LocationFormComponent, LocationFormDialogData, LocationFormDialogResult } from '../location-form/location-form.component';
import * as $ from "jquery";
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-location-panel',
  templateUrl: './location-panel.component.html',
  styleUrls: ['./location-panel.component.css']
})
export class LocationPanelComponent {

  public locations$ = this.loadLocationAsync();

  public constructor(private auth: AuthService, private http: HttpClient, private dialog: MatDialog){

  }

  public openFormCreateLocation(){
    //preapation data
    const data = new LocationFormDialogData()
    data.isUpdateMode = false;
    //open dialg
    const dialg = this.dialog.open(LocationFormComponent, {
      data: data,
      width: '400px',
    });

    //subscribe to afterClosed
    dialg.afterClosed().subscribe((result:LocationFormDialogResult) => {
      if(!result.isSuccess) return;
       this.locations$.then(arr => {
        var item = result.result;
          if(arr == undefined || item == null) return;

          var findCountry = arr.find(x => x.country == item?.country)
            if(findCountry == undefined){//create new and push
              arr.push(new Country(item.country, [item]))
            }else{//insert a new city into an existing country
              findCountry.cities.push(item);
            }
       })
    });
  }

  public openFormUpdateLocation(index:number) {
    console.log(index);
    var item = this.locations$.then(arr => {
      var item = arr != undefined ? arr[index]: undefined;
      if(item == undefined) return;
      var val = $(`#${item.country}`).val();
      return item.cities.find(x => x.city == val);
    });

    if(item == undefined) return;

    item.then(x => {
      //preapation data
      const data = new LocationFormDialogData()
      data.isUpdateMode = true;
      data.value = x??null;

      const dialg = this.dialog.open(LocationFormComponent, {
        data: data,
        width: '400px',
      });

      //subscribe to afterClosed
      dialg.afterClosed().subscribe((value:LocationFormDialogResult) => {
        if(!value.isSuccess) return;
        this.locations$.then(arr => {
          if(arr == undefined || value.result == null) return;
          var country = arr.find(x => x.cities.find(z => z.id == value.result?.id))
          if(country == undefined) return;

          if(value.result.country == country.country){//if country is not changed
            //just update date
            var item = country.cities.find(x => x.id == value.result?.id)
            if(item == undefined) return;
            item.city = value.result.city;
            return;//end
          }
          //else
          if(country.cities.length > 1)//if this is not last item
              country.cities.splice(country.cities.findIndex(x => x.id == value.result?.id), 1)
            else arr.splice(arr.indexOf(country), 1); //else deletes country from list

            var newCountry = arr.find(z => z.country == value.result?.country)
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
      let filterReult = x.reduce((group:any, item:any) => {
        const { country } = item
        group[country] = group[country] ?? []
        group[country].push(item)
        return group
      }, {});

      //Convert to regular object
      var list:Country[] = []
      Object.keys(filterReult).forEach(async item => {
        list.push(new Country(item, filterReult[item]))
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
