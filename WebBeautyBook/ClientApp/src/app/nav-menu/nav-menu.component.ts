import { Component } from '@angular/core';
import { UserDataModel } from '../models/UserDataModel';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  
  userData: UserDataModel;
  isAuth: boolean;

  constructor(private rout: Router, private auth: AuthService) {
    this.userData = new UserDataModel();
    this.isAuth = this.auth.hasToken();
 
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
  
}
