import { CompanyOpenHours } from "./CompanyOpenHours"
import { Location } from "./Location"
import { Service } from "./Service"

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
    services: Service[] = [];
    companyOpenHours: CompanyOpenHours[] = [];

}