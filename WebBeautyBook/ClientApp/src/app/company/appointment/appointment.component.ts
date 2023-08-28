import { Component, OnInit } from '@angular/core';
import { CalendarOptions, EventChangeArg, EventClickArg} from '@fullcalendar/core';
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

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css']
})
export class AppointmentComponent implements OnInit {
  
  public workers: WorkersSelect[] = [];
  public selectedWorker: string[] = [];
  public events: WorkerEventInput[] = [];

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
        }else {
          this.workers.push({worker: w, selected: false})
        }
    });
    var reservations = await this.getReservations();
    reservations?.forEach(item => this.addEvent(item))
    
    console.log(this.selectedWorker);

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

  public async toggleWorkerSelection(item: WorkersSelect) {
    if(!item.selected){
      var reservations = await this.getReservations();
      reservations?.forEach(item => this.addEvent(item))
    }else{
      this.events.forEach(x => {
        if(x.workerId == item.worker.id){
          this.removeEventById(x.id);
        }
      })
    }
    item.selected = !item.selected;
    console.log(this.selectedWorker)
  }

  private async getReservations():Promise<Reservation[]|undefined>{
    try {
      const queryParams = this.selectedWorker.join('&ids=');
      console.log(queryParams)
      return await this.http.get<Reservation[]>(`api/Reservation/Filter?ids=${queryParams}`, {
        headers: this.auth.getHeadersWithToken(),
      }).toPromise();
    }catch(error){
      console.log(error);
      return undefined;
    }
  }

  private addEvent(item: Reservation){
    if(this.events.findIndex(x => x.id == item.id) != -1) return;
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

  private removeEventById(id: string|undefined){
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

  private onReservationClick(item: EventClickArg){
    var data = this.converEventToReservation(item.event);
    if(data?.workerId != this.auth.getLocalUserDate()?.workerId){
      return;
    }
    const reservationDialog = this.dialogRef.open(ReservationDialogComponent, {
      width: "500px",
      data: {
        isUpdateMode: true,
        value: data
      } as ReservationDialogDate
    });
    reservationDialog.afterClosed().subscribe(data => {
      var result = data as ReservationDialogResult;
      if(result == null || result == undefined) return;

      if(!result.isSuccess || result.action == 'close') return;

      if(result.action == 'update' && result.value != null){
        var workerEI = this.events.find(x => x.id == result.value?.id)

        console.log()

        if(workerEI == undefined) return;
        var onlyDate = this.datePipe.transform(result.value.date?.toLocaleDateString(), 'yyyy-MM-dd');
        workerEI.title = result.value.description ?? 'reservation';
        workerEI.start = `${onlyDate}T${result.value.timeStart}`;
        workerEI.end = `${onlyDate}T${result.value.timeEnd}`;
        workerEI.workerId = result.value.workerId;

        this.calendar.events = [...this.events];

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