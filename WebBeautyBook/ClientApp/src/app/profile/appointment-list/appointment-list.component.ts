import { LiveAnnouncer } from '@angular/cdk/a11y';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { Appointment } from 'src/app/models/Appointment';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css']
})
export class AppointmentListComponent implements OnInit {

  public displayedColumns: string[] = [ 'date', 'time', 'service', 'status' ];
  public ELEMENT_DATA: Appointment[] = [];
  public dataSource = new MatTableDataSource<Appointment>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator|undefined;

  constructor(public auth: AuthService, private toastr: ToastrService, private http: HttpClient, private liveAnnouncer: LiveAnnouncer){
  }

  public async ngOnInit(){
    var appointments = await this.loadMyAppointment();
    if(appointments == undefined){
      return;
    }
    this.ELEMENT_DATA = appointments;
    this.dataSource = new MatTableDataSource<Appointment>(this.ELEMENT_DATA);
    console.log(this.ELEMENT_DATA);
  }

  private async loadMyAppointment():Promise<Appointment[]|undefined>{
    try {
      console.log('loadMyAppointment')
      return await this.http.get<Appointment[]>("api/Appointment/GetMyAppointments", {
        headers: this.auth.getHeadersWithToken()
      }).toPromise();
    }catch(error){
      console.log(error);
      return undefined;
    }
  }

}
