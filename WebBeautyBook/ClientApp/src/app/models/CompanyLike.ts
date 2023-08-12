import { BaseUser } from "./BaseUser"
import { Company } from "./Company"

export class CompanyLike{
    id:string = ''
    userId: string = ''
    user: BaseUser|undefined
   
    companyId : string = ''
    company: Company|undefined
}