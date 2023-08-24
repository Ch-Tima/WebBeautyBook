import { Component } from '@angular/core';
import { CalendarOptions, EventApi, EventChangeArg} from '@fullcalendar/core';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import { MatDialog } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';
import { ReservationDialogComponent } from '../reservation-dialog/reservation-dialog.component';
import { Company } from 'src/app/models/Company';
import { Reservation } from 'src/app/models/Reservation';
import { DatePipe } from '@angular/common';
import { title } from 'process';
import { WorkerEventInput } from 'src/app/models/WorkerEventInput';
import { ToastrService } from 'ngx-toastr';

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
    allDaySlot: false,//hide slot 'allDay'
    eventAllow: (item) => item.start.getDate() !== item.end.getDate() ? false : true,//prevent partial transfer to a new day
    initialView: 'dayGridMonth',
    initialEvents: [],
    weekends: true,
    editable: true,
    eventChange: (item) => this.eventChangeItem(item),
    snapDuration: '00:05:00',
    defaultAllDay: false,
    defaultAllDayEventDuration: null,
    //timeZone: 'UTC'
  };

  private events: WorkerEventInput[] = [];

  constructor(private toastr: ToastrService, private http: HttpClient, public auth: AuthService, private dialogRef : MatDialog, private datePipe: DatePipe){
    this.getMyReservations();
  }

  public addReservation(){
    const reservationDialog = this.dialogRef.open(ReservationDialogComponent, {
      width: "500px",
    });
    reservationDialog.afterClosed().subscribe(result => {
      console.log(result);
      if(result != null || result != undefined)
      {
        this.addEvent((result as Reservation));
      }
    });
  }
  
  public getMyReservations(){
    this.http.get<Reservation[]>("api/Reservation/GetMy", {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      result.forEach(item => this.addEvent(item));//add new items
    }, error => {
      console.log(error);
    });
  }

  private addEvent(item: Reservation){
    var onlyDate = item.date?.toString().replace(/T.*$/, '');
    var newEvent: WorkerEventInput = {
      id: item.id,
      color: "#50505080",
      start: `${onlyDate}T${item.timeStart}`,
      end: `${onlyDate}T${item.timeEnd}`,
      title: item.description,
      workerId: item.workerId,
    };
    this.events = [...this.events, newEvent];
    this.calendar.events = this.events;
  }

  private eventChangeItem(item: EventChangeArg){

    var newItem = new Reservation()
    if(item.event.start == null || item.event.end == null){
      this.toastr.error('Error event time')
      item.revert();
      return;
    }
    var newDate = item.event.start.getTime() > item.event.end.getTime() ? item.event.start.toISOString().substr(0, 10) : item.event.end.toISOString().substr(0, 10);
    var timeStart = this.datePipe.transform(item.event.start, 'HH:mm')??'';
    var timeEnd = this.datePipe.transform(item.event.end, 'HH:mm')??'';

    newItem.date = new Date(newDate);
    newItem.id = item.event.id;
    newItem.workerId = item.event.extendedProps['workerId'];
    newItem.timeStart = timeStart;
    newItem.timeEnd = timeEnd;
    newItem.description = title;

    this.http.post(`api/Reservation?id=${newItem.id}`, newItem, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.toastr.success('Reservation changed');
      }, error => {
        console.log(error);
        if(error.error.errors != undefined){ //erorr from model
          this.toastr.error(`${Object.values<any>(error.error.errors)[0][0]}`);
        }else{ //error from controller
          this.toastr.warning(`${error.error}`);
        }
        item.revert();
      }
    );
  }


}

/*

angular ts fullCalendare

при перемещение элемента в рамках одного дня меняется день

*/