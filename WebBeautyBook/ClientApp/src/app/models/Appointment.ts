import { BaseUser } from "./BaseUser"
import { Service } from "./Service";

export interface Appointment{
    id: string;

    date: Date|undefined;
    timeStart:string|undefined
    timeEnd:string|undefined

    note: string|undefined
    status: string|undefined
    userId: string|undefined
    baseUser: BaseUser|undefined

    workerId:string|undefined
    worker:Worker|undefined
    serviceId:string|undefined
    service:Service|undefined

    commentId: string|undefined

    type: 'Appointment'
}
