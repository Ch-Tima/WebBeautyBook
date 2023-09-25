import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NavMenuItem } from 'src/app/models/NavMenuItem';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-my-company-panel',
  templateUrl: './my-company-panel.component.html',
  styleUrls: ['./my-company-panel.component.css']
})
export class MyCompanyPanelComponent {

  public menu: string = ""; // Current menu selection
  public errorMessage: string = ""; // Error message, if any
  public navMenuItems: NavMenuItem[] = []; // Array of navigation menu items

  constructor(private auth: AuthService, private rout: Router){
    // Check if user is authenticated
    const user = this.auth.getLocalUserDate();
    if(!user || (!user.roles.includes('own_company') && !user.roles.includes('worker'))){
      this.rout.navigate(["login"]);// Redirect to the login page if not authenticated
    }else{
      this.InitNavLeftMenuItems();// Initialize navigation menu items
    }
  }

  // Handle navigation menu item selection
  public navLeftMenu(namePanel: string){
    this.menu = namePanel;
  }

  // Initialize navigation menu items
  private InitNavLeftMenuItems (){
    const mainMenu = new NavMenuItem("/assets/svg/home.svg", "Main", "Main")
    this.menu = mainMenu.value;
    this.navMenuItems.push(mainMenu);
    this.navMenuItems.push(new NavMenuItem("/assets/svg/appointment.svg", "Appointment", "Appointment"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/employees.svg", "Employees", "Employees"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/services.svg", "Services", "Services"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/clients.svg", "Clients", "Clients"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/list.svg", "History", "History"));
  }
}
