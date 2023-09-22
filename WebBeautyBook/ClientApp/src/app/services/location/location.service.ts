import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {LocationIPAPI} from "../../models/LocationIPAPI";

export const LOCATION: string = 'location';
export  const  IS_LOCATION_ACCESS: string = 'is_location_access';
export const LOCATION_DATE_UPDATE: string = 'location_date_update';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private http: HttpClient) {
  }

  // Asynchronously requests the user's location via IP address
  public async requestLocation(){
    return await this.http.get<LocationIPAPI>(`api/Location/getUserLocationViaIP`).toPromise();
  }

  // Sets the user's location in local storage
  public setLocation(l: LocationIPAPI){
    localStorage.setItem(LOCATION, JSON.stringify(l));
    localStorage.setItem(LOCATION_DATE_UPDATE, this.getDateWithoutTime(new Date()));
  }

  // Retrieves the user's location from local storage
  public getLocation():LocationIPAPI|null{
    const data = localStorage.getItem(LOCATION);
    return data == null ? null : JSON.parse(data) as LocationIPAPI;
  }

  // Checks if the stored location is outdated
  public isLocationOld():boolean{
    const dateFromStorage = localStorage.getItem(LOCATION_DATE_UPDATE);
    if(dateFromStorage == null) return true;//old
    const dateNow = this.getDateWithoutTime(new Date());
    return dateFromStorage == dateNow ? false : true;
  }

  // Checks if location access is granted
  public isLocationAccessGranted():boolean{
    return localStorage.getItem(IS_LOCATION_ACCESS) == 'true' ? true : false;
  }

  // Sets whether location access is granted in local storage
  public setLocationAccessGranted(b:boolean){
    localStorage.setItem(IS_LOCATION_ACCESS, b.toString());
  }

  // Returns the date in YYYY-MM-DD format without the time
  private getDateWithoutTime(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

}
