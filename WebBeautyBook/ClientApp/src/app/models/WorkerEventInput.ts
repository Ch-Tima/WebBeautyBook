import { EventInput } from "@fullcalendar/core";

export interface WorkerEventInput extends EventInput {
    workerId:string|undefined;
}