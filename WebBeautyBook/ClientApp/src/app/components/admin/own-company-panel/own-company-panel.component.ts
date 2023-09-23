import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { UserDataModel } from 'src/app/models/UserDataModel';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-own-company-panel',
  templateUrl: './own-company-panel.component.html',
  styleUrls: ['./own-company-panel.component.css']
})
export class OwnCompanyPanelComponent {

  users: UserDataModel[];

  constructor(private http: HttpClient, private auth: AuthService) {
    this.users = [];
    this.loadAllManagers();
  }

  loadAllManagers(){
    this.http.get<UserDataModel[]>("api/Auth?role=own_company", {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result =>{
        this.users = result;
      }, error => {
        console.log("ManagerPanelComponent -> loadAllManagers() -> ");
        console.log(error);
      }
    );
  }

  insertToList(user: UserDataModel){
    this.users.push(user);
  }

}
