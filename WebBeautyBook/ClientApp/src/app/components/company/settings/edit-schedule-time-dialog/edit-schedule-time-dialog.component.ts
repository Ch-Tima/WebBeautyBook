import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { publicDecrypt } from 'crypto';
import { ToastrService } from 'ngx-toastr';
import { CompanyOpenHours } from 'src/app/models/CompanyOpenHours';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-edit-schedule-time-dialog',
  templateUrl: './edit-schedule-time-dialog.component.html',
  styleUrls: ['./edit-schedule-time-dialog.component.css']
})
export class EditScheduleTimeDialogComponent {

  public time : CompanyOpenHours;

  constructor(private toast: ToastrService, private auth: AuthService, private http: HttpClient, private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : EditScheduleTimeDialogData){
    //Override close for sending results
    this.dialog.backdropClick().subscribe(r => this.cancel());
    if(data.value){
      this.time = {...data.value};
      this.time.openFrom = this.time.openFrom.substring(0, 5);
      this.time.openUntil = this.time.openUntil.substring(0, 5);
    }else{
      this.time = new CompanyOpenHours();
      this.time.openFrom = "08:00";
      this.time.openUntil = "16:00";
      this.time.dayOfWeek = this.data.dayOfWeek;
    }
  }

  public cancel(){
    this.dialog.close(new EditScheduleTimeDialogResult())
  }

  public setCloseStatus(){
    this.http.delete(`api/CompanyOpenHours?id=${this.time.id}`, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
        let resultDialog = new EditScheduleTimeDialogResult();
        resultDialog.status = 'close';
        resultDialog.value = this.data.value;
        this.toast.success("Save");
        this.dialog.close(resultDialog);
    }, ex => {
      this.toast.error(ex.error);
    });
  }
  
  public save(){
    this.http.post<any>(`api/CompanyOpenHours?id=${this.time.id}`, this.time, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(() => {
      let resultDialog = new EditScheduleTimeDialogResult();
      resultDialog.status = 'update';
      resultDialog.value = this.time;
      this.toast.success("Save");
      this.dialog.close(resultDialog);
    }, ex => {
      this.toast.error(ex.error, "Unexpected error");
    })
  }

  public insert(){
    this.http.put<CompanyOpenHours>(`api/CompanyOpenHours`, this.time, { 
      headers: this.auth.getHeadersWithToken() 
    }).subscribe(res => {
      let resultDialog = new EditScheduleTimeDialogResult();
      resultDialog.status = 'insert';
      resultDialog.value = res;
      this.toast.success("Created");
      this.dialog.close(resultDialog);
    }, ex => {
      this.toast.error(ex.error, "Unexpected error");
    });
  }

  public timeSet(){
    if(this.parseTimeString(this.time.openFrom) > this.parseTimeString(this.time.openUntil)){
      [this.time.openFrom, this.time.openUntil] = [this.time.openUntil, this.time.openFrom];
    }
  }

  private parseTimeString(timeString: string): Date {
    const [hours, minutes] = timeString.split(":").map(Number);
    const date = new Date();
    date.setHours(hours);
    date.setMinutes(minutes);
    date.setSeconds(0);
    console.log(date);
    return date;
}

}

export class EditScheduleTimeDialogResult {
  /**
    * @close set status closed
    * @update updated working hours
    * @insert add a new working day
    * @none didn't do anything (default)
  */
  public status: "close"|"update"|"insert"|"none" = "none";
  public value: CompanyOpenHours|undefined;
}

export class EditScheduleTimeDialogData {
  public mode: "create"|"update" = 'create';
  public value: CompanyOpenHours|undefined;
  public dayOfWeek: number = 0;
}