import { Component, signal } from '@angular/core';
import { CalendarOptions, EventInput } from '@fullcalendar/core';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import { INITIAL_EVENTS } from '../event-utils';
import { MatDialog } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';
import { ReservationDialogComponent } from '../reservation-dialog/reservation-dialog.component';
import { Company } from 'src/app/models/Company';
import { CompanyOpenHours } from 'src/app/models/CompanyOpenHours';
import { Reservation } from 'src/app/models/Reservation';

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
      right: 'dayGridMonth timeGridWeek timeGridDay'
    },
    initialView: 'dayGridMonth',
    initialEvents: [],
    weekends: true,
  };

  private events: EventInput[] = [];
  private company: Company = new Company

  constructor(private http: HttpClient, public auth: AuthService, private dialogRef : MatDialog){
    this.loadCompany();
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

  private async loadCompany(){
    await this.http.get<Company>(`api/Company/getMyCompany`, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      async (result) => {
        this.company = result;
      }, error => {
        console.log(error);
      }
    )
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

    const newEvent: EventInput = {
      id: createEventId(),
      color: "#50505080",
      start: `${onlyDate}T${item.timeStart}`,
      end: `${onlyDate}T${item.timeEnd}`,
      title: item.description
    };

    this.events = [...this.events, newEvent];
    this.calendar.events = this.events;

  }



}

function createEventId(): string {
  return (Math.random() + 1).toString(36).substring(2)
}
