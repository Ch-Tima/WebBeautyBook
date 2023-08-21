import { Component, signal } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import { INITIAL_EVENTS } from '../event-utils';
import { MatDialog } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';
import { ReservationDialogComponent } from '../reservation-dialog/reservation-dialog.component';

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css']
})
export class AppointmentComponent {
  
  calendarOptions = signal<CalendarOptions>({
    plugins: [
      interactionPlugin,
      dayGridPlugin,
      timeGridPlugin,
      listPlugin,
    ],
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
    },
    initialView: 'dayGridMonth',
    initialEvents: INITIAL_EVENTS,
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
  });

  constructor(private http: HttpClient, public auth: AuthService, private dialogRef : MatDialog){

  }

  public addReservation(){
    const reservationDialog = this.dialogRef.open(ReservationDialogComponent, {
      width: "500px",
      height: "250px"
    });
    reservationDialog.afterClosed().subscribe(result => {
      console.log("afterClosed");
    });
  }

}
