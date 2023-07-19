import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NavMenuItem } from 'src/app/models/NavMenuItem';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-my-company-panel',
  templateUrl: './my-company-panel.component.html',
  styleUrls: ['./my-company-panel.component.css']
})
export class MyCompanyPanelComponent {

  public menu:string = "";
  public errorMessage: string = "";

  public navMenuItems: NavMenuItem[] = [];

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
    this.menu = namePanel;
  }

  private InitNavLeftMenuItems (){
    var mainMenu = new NavMenuItem("/assets/svg/employees.svg", "Employees", "Employees");
    this.menu = mainMenu.value;
    this.navMenuItems.push(mainMenu);
    this.navMenuItems.push(new NavMenuItem("/assets/svg/appointment.svg", "Appointment", "Appointment"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/home.svg", "Main", "Main"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/services.svg", "Services", "Services"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/clients.svg", "Clients", "Clients"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/list.svg", "History", "History"));
  }
}
