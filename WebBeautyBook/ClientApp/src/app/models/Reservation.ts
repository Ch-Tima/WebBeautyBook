export interface Reservation {
    id: string|undefined;
    date: Date|undefined;
    timeStart: string|undefined;
    timeEnd: string|undefined;
    description: string|null;
    workerId: string|undefined;
    worker: Worker|undefined;

    type:'Reservation'

}