import {Component, OnInit} from '@angular/core';
import {UserDataModel} from '../../models/UserDataModel';
import {AuthService} from "../../services/auth/auth.service";
import {NavbarService} from "../../services/navbar/navbar.service";
import {catchError, forkJoin, map} from "rxjs";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  userData: UserDataModel;
  isAuth: boolean;

  constructor(private auth: AuthService, public nav: NavbarService) {
    this.userData = new UserDataModel();// Initialize user data and authentication status
    this.isAuth = this.auth.hasToken();
  }

  public async ngOnInit() {
    if (this.isAuth) {
      await this.loadUserData()// Load user data if authenticated
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

  private async loadUserData() {
    try {// Get user data from the server
      const result = await this.auth.getUserData().toPromise();
      if (result){
        this.userData = result;
        this.auth.saveUserData(result);
      }else this.signOut();
    } catch (err) {
      console.error(err);// Handle errors
      this.auth.signOut();// Sign out the user in case of an error
      return err;
    }
  }

}
