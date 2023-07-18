import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { CalendarOptions } from '@fullcalendar/core';
import { NavMenuItem } from 'src/app/models/NavMenuItem';
import { AuthService } from 'src/app/services/auth.service';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';
import { INITIAL_EVENTS } from '../event-utils';

@Component({
  selector: 'app-my-company-panel',
  templateUrl: './my-company-panel.component.html',
  styleUrls: ['./my-company-panel.component.css']
})
export class MyCompanyPanelComponent {

  public navMenuItems: NavMenuItem[] = [];
  public lastChangeMenuName:string = "";
  public errorMessage: string = "";

  //TEMP

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
    initialEvents: INITIAL_EVENTS, // alternatively, use the `events` setting to fetch from a feed
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    // select: this.handleDateSelect.bind(this),
    // eventClick: this.handleEventClick.bind(this),
    // eventsSet: this.handleEvents.bind(this)
    /* you can update a remote database when these fire:
    eventAdd:
    eventChange:
    eventRemove:
    */
  });

  //TEMP

  constructor(private auth: AuthService, private rout: Router){

    var user = this.auth.getLocalUserDate();
    if(user == null){
      this.rout.navigate(["login"]);
    }else{
      if(user.roles.filter(role => role == 'own_company').length == 0){
        rout.navigate(["/"]);
      }
    }
    this.InitNavLeftMenuItems();
  }

  public navLeftMenu(namePanel: string){
    this.lastChangeMenuName = namePanel;
  }

  private InitNavLeftMenuItems (){
    var mainMenu = new NavMenuItem("/assets/svg/home.svg", "Main", "Main");
    this.lastChangeMenuName = mainMenu.value;
    this.navMenuItems.push(mainMenu);
    this.navMenuItems.push(new NavMenuItem("/assets/svg/appointment.svg", "Appointment", "Appointment"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/employees.svg", "Employees", "Employees"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/services.svg", "Services", "Services"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/clients.svg", "Clients", "Clients"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/list.svg", "History", "History"));
  }
}
