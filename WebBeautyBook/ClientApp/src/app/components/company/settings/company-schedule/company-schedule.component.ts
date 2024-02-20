import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CompanyOpenHours } from 'src/app/models/CompanyOpenHours';
import { AuthService } from 'src/app/services/auth/auth.service';
import { TranslationService } from 'src/app/services/translation/translation.service';
import { EditScheduleTimeDialogComponent, EditScheduleTimeDialogData, EditScheduleTimeDialogResult } from 'src/app/components/company/settings/edit-schedule-time-dialog/edit-schedule-time-dialog.component'

@Component({
  selector: 'app-company-schedule',
  templateUrl: './company-schedule.component.html',
  styleUrls: ['./company-schedule.component.css']
})
export class CompanyScheduleComponent {

  // Array to store company open hours
  companyOpenHours: CompanyOpenHours[] = [];

  // Array representing days of the week
  week: string[] = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

  constructor(private http: HttpClient, private auth: AuthService, private router: Router, private toast: ToastrService, private translationService: TranslationService, private dialogRef : MatDialog,) {
    this.loadCompanyOpenHours();// Load company open hours on component initialization
  }

  // Function to format open hours for a specific day
  public getOpenHours(dayOfWeek: number):string{
    const result = this.companyOpenHours.find(x => x.dayOfWeek == dayOfWeek);
    if(result != undefined){
      return `${result.openFrom.length > 5 ? result.openFrom.substring(0, 5) : result.openFrom}-${result.openUntil.length > 5 ? result.openUntil.substring(0, 5) : result.openUntil}`;
    }else{
      return this.translationService.getTranslate("Closed");
    }
  }

  // Open dialog for modifying open hours
  public openTimeModificationDialog(val:number){
    let dayIndex = this.companyOpenHours.findIndex(x => x.dayOfWeek == val);
    let day = this.companyOpenHours[dayIndex];
    const dialog = this.dialogRef.open(EditScheduleTimeDialogComponent, {
      width: "550px",
      data: {
        mode: day ? 'update' : 'create',
        value: day,
        dayOfWeek: val
      } as EditScheduleTimeDialogData
    });
    dialog.afterClosed().subscribe((res:EditScheduleTimeDialogResult) => {
      switch (res.status) {
        case 'close':
          this.companyOpenHours.splice(dayIndex, 1); // Remove the day from companyOpenHours array if closed
          break;
        case 'update':
          if(res.value) {
            this.companyOpenHours[dayIndex] = res.value; // Update the day in companyOpenHours array
          }
          break;
        case 'insert':
          if(res.value)
            this.companyOpenHours = [...this.companyOpenHours, res.value]; // Add new open hours to companyOpenHours array
          break;
        case 'none':
          break;
      }
    })
  }

  // Load company open hours from the API
  private loadCompanyOpenHours(){
    this.http.get<CompanyOpenHours[]>(`/api/CompanyOpenHours?companyId=${this.auth.getUserFromLocalStorage()?.companyId}`).subscribe(res => {
      this.companyOpenHours = res;// Assign retrieved open hours to companyOpenHours array
      console.log(this.companyOpenHours);
    }, er => {
      console.log(er)// Log error if loading open hours fails
    })
  }

}
