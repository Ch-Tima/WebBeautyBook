import { Component, OnInit } from '@angular/core';
import { CalendarOptions, EventChangeArg, EventClickArg, EventInput} from '@fullcalendar/core';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import { MatDialog } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';
import { ReservationDialogComponent, ReservationDialogDate, ReservationDialogResult } from '../reservation-dialog/reservation-dialog.component';
import { Reservation } from 'src/app/models/Reservation';
import { DatePipe } from '@angular/common';
import { WorkerEventInput } from 'src/app/models/WorkerEventInput';
import { ToastrService } from 'ngx-toastr';
import { EventImpl } from '@fullcalendar/core/internal';
import { Worker } from '../../models/Worker';
import { Appointment } from 'src/app/models/Appointment';
import { ClientEventInput } from 'src/app/models/ClientEventInput';

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css']
})
export class AppointmentComponent implements OnInit {
  
  public workers: WorkersSelect[] = [];
  public selectedWorker: string[] = [];
  public events:ClientEventInput[]|WorkerEventInput[] = [];

  public calendar: CalendarOptions = {
    plugins: [
      interactionPlugin,
      dayGridPlugin,
      timeGridPlugin,
      listPlugin,
    ],
    headerToolbar: {
      left: 'prev next today',
      center: 'title',
      right: 'dayGridMonth timeGridWeek timeGridDay',
    },
    slotLabelFormat: {
      timeStyle: 'short',
      hour12: false,
    },
    slotMinTime: '00:00:00',
    slotMaxTime: '24:00:00',
    allDaySlot: false, //hide slot 'allDay'
    eventAllow: item => item.start.getDate() !== item.end.getDate() ? false : true, //prevent partial transfer to a new day
    eventClick: item => this.onReservationClick(item),
    eventChange: item => this.eventChangeItem(item),
    initialView: 'dayGridMonth',
    events: this.events,
    weekends: true,
    editable: true,
    snapDuration: '00:05:00',
    defaultAllDay: false,
    defaultAllDayEventDuration: null,
  };

  constructor(public auth: AuthService, private toastr: ToastrService, private http: HttpClient, private dialogRef : MatDialog, private datePipe: DatePipe){
  }

  public async ngOnInit(){
    var workers = await this.getWorkers();
    var user = this.auth.getLocalUserDate();
    workers?.forEach(w => {
        if(w.id == user?.workerId) {
          this.workers.push({ worker: w, selected: true })
          this.selectedWorker.push(w.id)
        }else this.workers.push({worker: w, selected: false})
    });
    (await this.getReservations())?.forEach(r => (r.type = 'Reservation', this.addEvent(r)));
    (await this.getAppointment())?.forEach(a => (a.type = 'Appointment', this.addEvent(a)))
  }

  public addReservation(){
    const reservationDialog = this.dialogRef.open(ReservationDialogComponent, {
      width: "500px",
      data: { isUpdateMode: false } as ReservationDialogDate
    });
    reservationDialog.afterClosed().subscribe(result => {
      if(result == null) return;
      var reservation = (result as ReservationDialogResult).value;
      if(reservation != null || reservation != undefined){
        this.addEvent(reservation);
      }
    });
  }

  /**
   * Method for toggling worker selection.
   * @param item Object representing the selected worker.
   */
  public async toggleWorkerSelection(item: WorkersSelect) {
    if(!item.selected){
      // If the worker was not selected, add events (reservations and appointments) to the calendar.
      // Mark reservations and appointments with their respective types and add them to the calenda
      (await this.getReservations())?.forEach(r => (r.type = 'Reservation', this.addEvent(r)));
      (await this.getAppointment())?.forEach(a => (a.type = 'Appointment', this.addEvent(a)));
    }else{
      // If the worker was selected, remove all events associated with that worker.
      // This is done by iterating through all events in the calendar and removing those belonging to the selected worker.
      this.events.forEach(x => {
        if(x.workerId == item.worker.id){
          this.removeEventById(x.id);
        }
      })
    }
    // Invert the worker selection flag (if it was selected, make it unselected, and vice versa).
    item.selected = !item.selected;
  }

  private addEvent(item: Reservation|Appointment){
    if(this.events.findIndex(x => x.id == item.id) != -1) return;
    this.events = [...this.events, this.convertEventToMyEventInput(item)];
    this.calendar.events = this.events;
  }

  private removeEventById(id: string|undefined){
    this.events = this.events.filter(x => x.id != id);
    this.calendar.events = this.events;
  }

  private eventChangeItem(item: EventChangeArg){
    if(item.event.extendedProps['userId'] != undefined && item.event.extendedProps['serviceId'] != undefined){
      item.revert();//temporary ban on movement 'ClientEventInput' user Appointment
      return;
    }

    var reservation = this.converEventToReservation(item.event);
    if(reservation == null){
      this.toastr.error('Error event time')
      item.revert();
      return;
    }

    if(reservation.workerId != this.auth.getLocalUserDate()?.workerId){
      item.revert();
      this.toastr.warning(`This is not your booking.`, undefined, { closeButton: true, timeOut: 1000 });
      return;
    }

    this.http.post(`api/Reservation?id=${reservation.id}`, reservation, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.toastr.success('Reservation changed', undefined, { timeOut: 1000 });
        var w = this.events.find(x => x.id == item.event.id);
        if(w != undefined){
          w.start = item.event.start??undefined;
          w.end = item.event.end??undefined;
        }else{
          console.log("error");
          item.revert();
        }
      }, error => {
        console.log(error);
        if(error.error.errors != undefined){ //erorr from model
          this.toastr.error(`${Object.values<any>(error.error.errors)[0][0]}`, undefined, { closeButton: true });
        }else{ //error from controller
          this.toastr.warning(`${error.error}`, undefined, { closeButton: true });
        }
        item.revert();
      }
    );
  }

  /**
   * Handles the click event on a reservation item in the calendar.
   * @param item - The EventClickArg representing the clicked event.
   */
  private onReservationClick(item: EventClickArg){
    if(item.event.extendedProps['userId'] != undefined && item.event.extendedProps['serviceId'] != undefined){
      return;//temporary ban on movement 'ClientEventInput' user Appointment
    }
    //Convert the clicked event to a Reservation objec
    var data = this.converEventToReservation(item.event);
    //Check if the workerId of the clicked event matches the authenticated user's workerId
    if(data?.workerId != this.auth.getLocalUserDate()?.workerId) return;
    const reservationDialog = this.dialogRef.open(ReservationDialogComponent, {
      width: "500px",
      data: { isUpdateMode: true, value: data } as ReservationDialogDate
    });
    //Subscribe to the dialog's closed event
    reservationDialog.afterClosed().subscribe(data => {
      var result = data as ReservationDialogResult;
      //check if the dialog result indicates success and an action is performed
      if((result == null || result == undefined) || (!result.isSuccess || result.action == 'close')) return;

      if(result.action == 'update' && result.value != null){
        var workerEI = this.events.find(x => x.id == result.value?.id)
        if(workerEI == undefined) return;
        var onlyDate = this.datePipe.transform(result.value.date?.toLocaleDateString(), 'yyyy-MM-dd');//format the date part
        //update the worker event's properties
        workerEI.title = result.value.description ?? 'reservation';
        workerEI.start = `${onlyDate}T${result.value.timeStart}`;
        workerEI.end = `${onlyDate}T${result.value.timeEnd}`;
        workerEI.workerId = result.value.workerId;
        this.calendar.events = [...this.events]; //update the 'calendar.events' with the updated 'events' array
      }
      if(result.action == 'remove') this.removeEventById(item.event.id);//remove the clicked event by its ID

    });
  }

  /**
   * Converts an EventImpl object to a Reservation object.
   * @param event - The EventImpl object to convert.
   * @returns A Reservation object or null if the conversion is not possible.
  */
  private converEventToReservation(event: EventImpl):Reservation|null{
    if(event.start == null || event.end == null) return null;// Check if event.start or event.end is null; return null if either is null
    return {//Create a Reservation object based on the properties of the EventImpl object
      id: event.id,
      date: new Date(event.start.toDateString()),
      workerId: event.extendedProps['workerId'],
      worker: undefined,
      timeStart: this.datePipe.transform(event.start, 'HH:mm')??'',
      timeEnd: this.datePipe.transform(event.end, 'HH:mm')??'',
      description: event.title.length == 0 ? null : event.title,
    } as Reservation;
  }

  /**
   * Converts a Reservation or Appointment object into a WorkerEventInput or ClientEventInput, respectively.
   * @param item - The Reservation or Appointment object to convert.
   * @returns A WorkerEventInput if the item is a Reservation, or a ClientEventInput if it's an Appointment.
   */
  private convertEventToMyEventInput(item: Reservation|Appointment):WorkerEventInput|ClientEventInput{
    var onlyDate = item.date?.toString().replace(/T.*$/, '');  
    var event: EventInput = { //Create an EventInput object with common properties
      id: item.id,
      color: "#50505080",
      start: `${onlyDate}T${item.timeStart}`,
      end: `${onlyDate}T${item.timeEnd}`,
      title: (item.type == 'Reservation') ? (item.description??'reservation') : item.note,
    };

    if(item.type == 'Reservation'){// If the item is a Reservation, cast the event as a WorkerEventInp
      var workerEvent = event as WorkerEventInput;
      workerEvent.workerId = item.workerId
      return workerEvent;
    }else{// If the item is an Appointment, cast the event as a ClientEventInput
      var clientEvent = event as ClientEventInput;
      clientEvent.workerId = item.workerId;
      clientEvent.serviceId = item.serviceId;
      clientEvent.userId = item.userId;
      return clientEvent;
    }
  }

  /**
   * Retrieves reservations for selected workers from the API.
   * @returns A promise that resolves to an array of Reservations or undefined on error.
   */
  private async getReservations():Promise<Reservation[]|undefined>{
    try {
      const queryParams = this.selectedWorker.join('&ids=');
      return await this.http.get<Reservation[]>(`api/Reservation/Filter?ids=${queryParams}`, {
        headers: this.auth.getHeadersWithToken(),
      }).toPromise();
    }catch(error){
      console.log(error);
      return undefined;
    }
  }

  /**
   * Retrieves appointments for selected workers from the API.
   * @returns A promise that resolves to an array of Appointments or undefined on error.
   */
  private async getAppointment():Promise<Appointment[]|undefined>{
    try {
      const queryParams = this.selectedWorker.join('&ids=');
      return await this.http.get<Appointment[]>(`api/Appointment/GetAppointmentsForMyCompany?ids=${queryParams}`, {
        headers: this.auth.getHeadersWithToken(),
      }).toPromise();
    }catch(error){
      console.log(error);
      return undefined;
    }
  }

 /**
  * Makes a request to the API and gets a list of employees.
  * @return Promise<Worker[]|undefined>
  */
  private async getWorkers():Promise<Worker[]|undefined>{
    try {
      return await this.http.get<Worker[]>('api/Company/getWorkers', {
        headers: this.auth.getHeadersWithToken(),
      }).toPromise();
    }catch(error){
      console.log(error);
      return undefined;
    }
  }

}
class WorkersSelect{
  worker:Worker = new Worker
  selected: boolean = false
}