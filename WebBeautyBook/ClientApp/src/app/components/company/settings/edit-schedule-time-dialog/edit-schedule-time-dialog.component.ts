import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { CompanyOpenHours } from 'src/app/models/CompanyOpenHours';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-edit-schedule-time-dialog',
  templateUrl: './edit-schedule-time-dialog.component.html',
  styleUrls: ['./edit-schedule-time-dialog.component.css']
})
export class EditScheduleTimeDialogComponent {

    // Represents the company open hours being edited
    public editedCompanyOpenHours: CompanyOpenHours;

  constructor(private toast: ToastrService, private auth: AuthService, private http: HttpClient, private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : EditScheduleTimeDialogData){
    //Override close for sending results
    this.dialog.backdropClick().subscribe(r => this.cancel());
    //Initialize time based on provided data or defaults
    if(data.value){
      this.editedCompanyOpenHours = {...data.value};
      this.editedCompanyOpenHours.openFrom = this.editedCompanyOpenHours.openFrom.substring(0, 5);
      this.editedCompanyOpenHours.openUntil = this.editedCompanyOpenHours.openUntil.substring(0, 5);
    }else{
      this.editedCompanyOpenHours = new CompanyOpenHours();
      this.editedCompanyOpenHours.openFrom = "08:00";
      this.editedCompanyOpenHours.openUntil = "16:00";
      this.editedCompanyOpenHours.dayOfWeek = this.data.dayOfWeek;
    }
  }

  // Close the dialog with a default result
  public cancel(){
    this.dialog.close(new EditScheduleTimeDialogResult())
  }

  // Close the schedule time
  public setCloseStatus(){
    // Make a DELETE request to remove the schedule time
    this.http.delete(`api/CompanyOpenHours?id=${this.editedCompanyOpenHours.id}`, {
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
  
  // Save changes to the schedule time
  public save(){
    // Make a POST request to update the schedule time
    this.http.post<any>(`api/CompanyOpenHours?id=${this.editedCompanyOpenHours.id}`, this.editedCompanyOpenHours, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(() => {
      let resultDialog = new EditScheduleTimeDialogResult();
      resultDialog.status = 'update';
      resultDialog.value = this.editedCompanyOpenHours;
      this.toast.success("Save");
      this.dialog.close(resultDialog);
    }, ex => {
      this.toast.error(ex.error, "Unexpected error");
    })
  }

  // Insert a new schedule time
  public insert(){
    // Make a PUT request to add a new schedule time
    this.http.put<CompanyOpenHours>(`api/CompanyOpenHours`, this.editedCompanyOpenHours, { 
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

  // Ensure that openFrom is before openUntil
  public timeSet(){
    if(this.parseTimeString(this.editedCompanyOpenHours.openFrom) > this.parseTimeString(this.editedCompanyOpenHours.openUntil)){
      [this.editedCompanyOpenHours.openFrom, this.editedCompanyOpenHours.openUntil] = [this.editedCompanyOpenHours.openUntil, this.editedCompanyOpenHours.openFrom];
    }
  }

// Parse time string into Date object
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

// Data for the edit schedule time dialog
export class EditScheduleTimeDialogData {
  public mode: "create"|"update" = 'create';
  public value: CompanyOpenHours|undefined;
  public dayOfWeek: number = 0;
}