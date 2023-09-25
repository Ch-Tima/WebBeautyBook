import {Component, OnInit} from '@angular/core';
import { Worker } from "../../../models/Worker"
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { InvitationEmployeeComponent } from '../invitation-employee/invitation-employee.component';
import {AuthService} from "../../../services/auth/auth.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-employees-panel',
  templateUrl: './employees-panel.component.html',
  styleUrls: ['./employees-panel.component.css']
})
export class EmployeesPanelComponent implements OnInit{

  public workers: Worker[]|undefined// Array to store worker data

  constructor(private auth: AuthService, private http: HttpClient, private dialogRef : MatDialog, private toast: ToastrService){
  }

  public async ngOnInit() {
    await this.loadWorkers();// Load worker data when the component is initialized
  }

  // Load worker data from the server
  private async loadWorkers(){
    try {
      const result = await this.http.get<Worker[]>("api/Company/getWorkers", {
        headers: this.auth.getHeadersWithToken()// Include authorization headers
      }).toPromise();
      this.workers = result??[];// Update the workers array with the loaded data
    }catch (e) {
      console.error(e);
      this.toast.error("Request error, see console for details");
    }
  }

  // Open the invitation employee dialog
  public onInvitationEmployee(){
    this.dialogRef.open(InvitationEmployeeComponent, {
      width: "400px"
    });
  }

}
