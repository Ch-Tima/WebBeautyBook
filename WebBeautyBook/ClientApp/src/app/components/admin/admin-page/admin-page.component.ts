import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NavMenuItem } from 'src/app/models/NavMenuItem';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent {

  public menu: string = '';

  public navMenuItems: NavMenuItem[] = [];

  constructor(private auth: AuthService, private rout: Router){
    const user = this.auth.getLocalUserDate();
    if(!user){// Check if a user is authenticated
      this.rout.navigate(["login"]);
    }else if(user.roles.filter(role => role == 'admin').length == 0){
      this.rout.navigate(["/"]);
    }else {
      this.InitNavLeftMenuItems(); // Initialize the navigation menu items
    }
  }
  public navLeftMenu(menu: string){
    this.menu = menu;
  }
  private InitNavLeftMenuItems (){
    let def = new NavMenuItem("/assets/svg/location.svg", "Location", "Location");
    this.menu = def.value;
    this.navMenuItems.push(def);
    this.navMenuItems.push(new NavMenuItem("/assets/svg/clients.svg", "Own company", "OwnCompany"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/company.svg", "Company", "Company"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/categoty.svg", "Categoty", "Categoty"));
  }

}
