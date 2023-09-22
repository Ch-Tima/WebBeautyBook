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

  public menu: string = "Categoty";

  public navMenuItems: NavMenuItem[] = [];

  constructor(private auth: AuthService, private rout: Router){
    var user = this.auth.getLocalUserDate();
    if(user == null){
      this.rout.navigate(["login"]);
    }else{
      if(user.roles.filter(role => role == 'admin').length == 0){
        rout.navigate(["/"]);
      }
    }

    this.InitNavLeftMenuItems();
  }

  public navLeftMenu(menu: string){
    this.menu = menu;
  }

  private InitNavLeftMenuItems (){
    var def = new NavMenuItem("/assets/svg/location.svg", "Location", "Location");
    this.menu = def.value;
    this.navMenuItems.push(def);
    this.navMenuItems.push(new NavMenuItem("/assets/svg/clients.svg", "Own company", "OwnCompany"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/company.svg", "Company", "Company"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/categoty.svg", "Categoty", "Categoty"));
  }

}
