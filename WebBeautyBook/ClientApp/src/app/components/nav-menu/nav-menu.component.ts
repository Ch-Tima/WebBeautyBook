import { Component } from '@angular/core';
import { UserDataModel } from '../../models/UserDataModel';
import { Router } from '@angular/router';
import {AuthService} from "../../services/auth/auth.service";
import {NavbarService} from "../../services/navbar/navbar.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  userData: UserDataModel;
  isAuth: boolean;

  constructor(private rout: Router, private auth: AuthService, public nav: NavbarService) {
    this.userData = new UserDataModel();
    this.isAuth = this.auth.hasToken();
    if (this.isAuth) {
      this.getUserData();
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }


  public signOut() {
    this.auth.signOut();
  }

  private getUserData() {
    this.auth.getUserData().subscribe(
      result => {
        this.userData = result;
        this.auth.saveUserData(result);
      }, error => {
        alert("Error: nav-menu");
        console.log(error);
        this.auth.signOut();
      });
  }

}
