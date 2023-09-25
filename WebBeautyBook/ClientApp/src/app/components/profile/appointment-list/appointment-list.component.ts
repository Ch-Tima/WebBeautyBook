import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';;
import { Appointment } from 'src/app/models/Appointment';
import { AuthService } from "../../../services/auth/auth.service";
import { AppointmentDialogComponent, AppointmentDialogDate, AppointmentDialogResult } from "../../publicCompany/appointment-dialog/appointment-dialog.component";

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css']
})
export class AppointmentListComponent implements OnInit {

  public displayedColumns: string[] = [ 'date', 'time', 'service', 'status' ];// Define displayed columns for the table
  public appointments: Appointment[]|undefined;// Initialize appointments array
  public dataSource = new MatTableDataSource<Appointment>([]);// Create a MatTableDataSource for the table

  constructor(private cdRef: ChangeDetectorRef, private auth: AuthService, private dialogRef : MatDialog, private http: HttpClient){
  }

  public async ngOnInit(){
    // Load user's appointments asynchronously
    const appointments = await this.loadMyAppointment();
    if(appointments == undefined) return;
    this.appointments = appointments;
    this.refresh(this.appointments)
  }

  public openDialog(val: Appointment){
    // Open the appointment dialog
    const reservationDialog = this.dialogRef.open(AppointmentDialogComponent, {
      width: "500px",
      data: { isUpdateMode: true, value: val } as AppointmentDialogDate
    });
    reservationDialog.afterClosed().subscribe((value) => {
      if(!value) return;
      const result = value as AppointmentDialogResult;
      if(!result.isSuccess || result.action == 'close') return;
      if(result.action == 'remove' && this.appointments != undefined){
        // Remove the appointment from the list
        this.appointments = this.appointments.filter(x => x.id != val.id);
        this.refresh(this.appointments);
      }
    });
  }

  // Update the MatTableDataSource with the new data
  private refresh(list: Appointment[]) {
    this.dataSource.data = list;
    this.cdRef.detectChanges();
  }

  // Load user's appointments from the API using an async/await pattern
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
