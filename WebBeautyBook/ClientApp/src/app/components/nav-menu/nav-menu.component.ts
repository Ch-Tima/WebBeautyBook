import { Component } from '@angular/core';
import { UserDataModel } from '../../models/UserDataModel';
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

  constructor(private auth: AuthService, public nav: NavbarService) {
    this.userData = new UserDataModel();// Initialize user data and authentication status
    this.isAuth = this.auth.hasToken();
    if (this.isAuth) {
      this.getUserData();// Load user data if authenticated
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }


  public signOut() {
    this.auth.signOut();// Sign out the user
  }

  private getUserData() {
    // Get user data from the server
    this.auth.getUserData().subscribe(
      result => {
        this.userData = result;// Get user data from the server
        this.auth.saveUserData(result);
      }, error => {
        console.error(error);// Handle errors
        this.auth.signOut();// Sign out the user in case of an error
      });
  }

}
