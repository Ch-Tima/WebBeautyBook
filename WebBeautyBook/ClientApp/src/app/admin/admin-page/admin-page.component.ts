import { Component } from '@angular/core';
import { NavMenuItem } from 'src/app/models/NavMenuItem';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent {

  public menu: string = "Categoty";

  public navMenuItems: NavMenuItem[] = [];

  constructor(){
    this.InitNavLeftMenuItems();
  }

  public navLeftMenu(menu: string){
    this.menu = menu;
  }

  private InitNavLeftMenuItems (){
    var def = new NavMenuItem("/assets/svg/categoty.svg", "Categoty", "Categoty");
    this.menu = def.value;
    this.navMenuItems.push(def);
    this.navMenuItems.push(new NavMenuItem("/assets/svg/clients.svg", "Own company", "OwnCompany"));
    this.navMenuItems.push(new NavMenuItem("/assets/svg/company.svg", "Company", "Company"));
  }

}
