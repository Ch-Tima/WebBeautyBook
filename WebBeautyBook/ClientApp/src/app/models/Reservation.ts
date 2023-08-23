export class Reservation {
    public id: string|undefined;
    public date: Date|undefined;
    public timeStart: string|undefined;
    public timeEnd: string|undefined;
    public description: string = '';

    public workerId: string|undefined;
    public worker: Worker|undefined;
}