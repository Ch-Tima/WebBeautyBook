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

  public menu: string = "";
  public navMenuItems: NavMenuItem[] = [];

  constructor(private auth: AuthService, private rout: Router){
    var user = this.auth.getLocalUserDate();
    if(user == null){
      this.rout.navigate(["login"]);
      return;
    }
    this.InitNavLeftMenuItems();
  }

  public navLeftMenu(menu: string){
    this.menu = menu;
  }

  private InitNavLeftMenuItems (){
    var def = new NavMenuItem("/assets/svg/appointment.svg", "Appointments", "Appointments")
    this.menu = def.value;
    this.navMenuItems.push(def);
    this.navMenuItems.push(new NavMenuItem("/assets/svg/hearts.svg", "Favourites", "Favourites"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/settings.svg", "Account&Settings", "Settings"));
  }

}
