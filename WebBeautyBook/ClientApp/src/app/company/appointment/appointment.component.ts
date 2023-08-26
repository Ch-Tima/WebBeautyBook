import { Component } from '@angular/core';
import { CalendarOptions, EventApi, EventChangeArg, EventClickArg} from '@fullcalendar/core';
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

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css']
})
export class AppointmentComponent {
  
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
      //timeZone: 'UTC'
    },
    slotMinTime: '00:00:00',
    slotMaxTime: '24:00:00',
    allDaySlot: false, //hide slot 'allDay'
    eventAllow: (item) => item.start.getDate() !== item.end.getDate() ? false : true, //prevent partial transfer to a new day
    eventClick: (item) => this.onReservationClick(item),
    eventChange: item => this.eventChangeItem(item),
    initialView: 'dayGridMonth',
    initialEvents: [],
    weekends: true,
    editable: true,
    snapDuration: '00:05:00',
    defaultAllDay: false,
    defaultAllDayEventDuration: null,
  };

  public events: WorkerEventInput[] = [];

  constructor(private toastr: ToastrService, private http: HttpClient, public auth: AuthService, private dialogRef : MatDialog, private datePipe: DatePipe){
    this.getMyReservations();
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
  
  public getMyReservations(){
    this.http.get<Reservation[]>("api/Reservation/GetMy", {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => result.forEach(item => this.addEvent(item)), //add new items
      error => console.log(error));
  }

  private addEvent(item: Reservation){
    var onlyDate = item.date?.toString().replace(/T.*$/, '');
    var newEvent: WorkerEventInput = {
      id: item.id,
      color: "#50505080",
      start: `${onlyDate}T${item.timeStart}`,
      end: `${onlyDate}T${item.timeEnd}`,
      title: item.description??'reservation',
      workerId: item.workerId,
    };
    this.events = [...this.events, newEvent];
    this.calendar.events = this.events;
  }

  private removeEventById(id: string){
    this.events = this.events.filter(x => x.id != id);
    this.calendar.events = this.events;
  }

  private eventChangeItem(item: EventChangeArg){
    var reservation = this.converEventToReservation(item.event);

    if(reservation == null){
      this.toastr.error('Error event time')
      item.revert();
      return;
    }

    this.http.post(`api/Reservation?id=${reservation.id}`, reservation, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.toastr.success('Reservation changed', undefined, { timeOut: 1000 });
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

  private onReservationClick(item: EventClickArg){
    const reservationDialog = this.dialogRef.open(ReservationDialogComponent, {
      width: "500px",
      data: {
        isUpdateMode: true,
        value: this.converEventToReservation(item.event)
      } as ReservationDialogDate
    });
    reservationDialog.afterClosed().subscribe(data => {
      var result = data as ReservationDialogResult;
      if(result == null || result == undefined) return;

      if(!result.isSuccess || result.action == 'close') return;

      if(result.action == 'update'){
        
      }
      if(result.action == 'remove'){
        this.removeEventById(item.event.id);
      }

    });
  }

  private converEventToReservation(event: EventImpl):Reservation|null{
    if(event.start == null || event.end == null) return null;
    var item: Reservation = {
      id: event.id,
      date: new Date(event.start.toDateString()),
      workerId: event.extendedProps['workerId'],
      worker: undefined,
      timeStart: this.datePipe.transform(event.start, 'HH:mm')??'',
      timeEnd: this.datePipe.transform(event.end, 'HH:mm')??'',
      description: event.title.length == 0 ? null : event.title
    };
    return item;
  }

}