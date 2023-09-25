import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';
import { NavMenuItem } from '../../../models/NavMenuItem';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent {

  // Initialize public properties
  public menu: string = ""; // Represents the currently selected menu item
  public navMenuItems: NavMenuItem[] = []; // Array to hold navigation menu items
  constructor(private auth: AuthService, private rout: Router){
    const user = this.auth.getLocalUserDate(); // Check if a user is logged in
    if(user == null){// If no user is logged in, redirect to the login page
      this.rout.navigate(["login"]);
      return;
    }
    this.InitNavLeftMenuItems();// Check if a user is logged in
  }

  public navLeftMenu(menu: string){
    this.menu = menu;
  }

  // Method to initialize navigation menu items
  private InitNavLeftMenuItems (){
    // Create a default menu item and set it as the active menu
    const def = new NavMenuItem("/assets/svg/appointment.svg", "Appointments", "Appointments")
    this.menu = def.value;
    this.navMenuItems.push(def);// Push the default menu item to the array
    this.navMenuItems.push(new NavMenuItem("/assets/svg/hearts.svg", "Favourites", "Favourites"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/settings.svg", "Account&Settings", "Settings"));
  }

}
