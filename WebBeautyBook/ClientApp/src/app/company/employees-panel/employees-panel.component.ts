import { Component, Inject } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Worker } from "../../models/Worker"
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-employees-panel',
  templateUrl: './employees-panel.component.html',
  styleUrls: ['./employees-panel.component.css']
})
export class EmployeesPanelComponent {


  public workers: Worker[] = [];

  constructor(private auth: AuthService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string){
    this.loadWorkers();
  }

  private loadWorkers(){
    this.http.get<Worker[]>(this.baseUrl + "api/Company/getWorkers", {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.workers = result;
      }, error => {
        console.log(error);
        alert("EmployeesPanelComponent -> error -> loadWorkers()")
      }
    );
  } 

}
