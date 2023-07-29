import { Location } from "./Location"

export class Company{

    id:string = ''
    name:string = ''
    phonenumber:string = ''
    email: string = ''
    logo: string = ''
    address : string = ''
    isVisibility: boolean = false
    locationId:string = ''
    location: Location = new Location;
}