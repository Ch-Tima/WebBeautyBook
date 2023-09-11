import { LiveAnnouncer } from '@angular/cdk/a11y';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { Appointment } from 'src/app/models/Appointment';
import { AppointmentDialogComponent, AppointmentDialogDate, AppointmentDialogResult } from 'src/app/publicCompany/appointment-dialog/appointment-dialog.component';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css']
})
export class AppointmentListComponent implements OnInit {

  public displayedColumns: string[] = [ 'date', 'time', 'service', 'status' ];
  public appointments: Appointment[]|undefined;
  public dataSource = new MatTableDataSource<Appointment>([]);

  constructor(private cdRef: ChangeDetectorRef, private auth: AuthService, private toastr: ToastrService, private dialogRef : MatDialog, private http: HttpClient, private liveAnnouncer: LiveAnnouncer, ){
  }

  public async ngOnInit(){
    var appointments = await this.loadMyAppointment();
    if(appointments == undefined){
      return;
    }
    this.appointments = appointments;
    this.refresh(this.appointments)
  }

  public openDiallog(val: Appointment){
    const reservationDialog = this.dialogRef.open(AppointmentDialogComponent, {
      width: "500px",
      data: { isUpdateMode: true, value: val } as AppointmentDialogDate
    });
    reservationDialog.afterClosed().subscribe((value) => {
      if(value == null || value == undefined) return;
      var result = value as AppointmentDialogResult;
      if(!result.isSuccess || result.action == 'close') return;
      if(result.action == 'remove' && this.appointments != undefined){
        this.appointments = this.appointments.filter(x => x.id != val.id);
        this.refresh(this.appointments);
      }
    });
  }

  private refresh(list: Appointment[]) {
    this.dataSource.data = list;
    this.cdRef.detectChanges();
  }

  private async loadMyAppointment():Promise<Appointment[]|undefined>{
    try {
      return await this.http.get<Appointment[]>("api/Appointment/GetMyAppointments", {
        headers: this.auth.getHeadersWithToken()
      }).toPromise();
    }catch(error){
      console.log(error);
      return undefined;
    }
  }

}
