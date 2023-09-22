import { Injectable } from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {HttpClient} from "@angular/common/http";
import {LocationIPAPI} from "../../models/LocationIPAPI";
import {Subject} from "rxjs";

export const LOCATION: string = 'location';
export  const  IS_LOCATION_ACCESS: string = 'is_location_access';
export const LOCATION_DATE_UPDATE: string = 'location_date_update';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private toastr: ToastrService, private http: HttpClient) {
  }
  public async requestLocation(){
    console.warn("requestLocation");
    return await this.http.get<LocationIPAPI>(`api/Location/getUserLocationViaIP`).toPromise();
  }

  public setLocation(l: LocationIPAPI){
    localStorage.setItem(LOCATION, JSON.stringify(l));
    localStorage.setItem(LOCATION_DATE_UPDATE, this.getDateWithoutTime(new Date()));
  }

  public  getLocation():LocationIPAPI|null{
    const data = localStorage.getItem(LOCATION);
    return data == null ? null : JSON.parse(data) as LocationIPAPI;
  }

  public isLocationOld():boolean{
    const dateFromStorage = localStorage.getItem(LOCATION_DATE_UPDATE);
    if(dateFromStorage == null) return true;//old
    const dateNow = this.getDateWithoutTime(new Date());
    return dateFromStorage == dateNow ? false : true;
  }

  public isLocationAccessGranted():boolean{
    return localStorage.getItem(IS_LOCATION_ACCESS) == 'true' ? true : false;
  }

  public setLocationAccessGranted(b:boolean){
    localStorage.setItem(IS_LOCATION_ACCESS, b.toString());
  }

  private getDateWithoutTime(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

}
