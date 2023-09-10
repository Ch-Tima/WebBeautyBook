import { EventInput } from "@fullcalendar/core";

export interface ClientEventInput extends EventInput {
    userId:string|undefined;
    workerId:string|undefined;
    serviceId:string|undefined;
}