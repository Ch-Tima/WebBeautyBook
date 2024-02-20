import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CompanyOpenHours } from 'src/app/models/CompanyOpenHours';
import { AuthService } from 'src/app/services/auth/auth.service';
import { TranslationService } from 'src/app/services/translation/translation.service';
import { EditScheduleTimeDialogComponent, EditScheduleTimeDialogResult } from 'src/app/components/company/settings/edit-schedule-time-dialog/edit-schedule-time-dialog.component'

@Component({
  selector: 'app-company-schedule',
  templateUrl: './company-schedule.component.html',
  styleUrls: ['./company-schedule.component.css']
})
export class CompanyScheduleComponent {

  companyOpenHours: CompanyOpenHours[] = []

  week: string[] = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']

  constructor(private http: HttpClient, private auth: AuthService, private router: Router, private toast: ToastrService, private translationService: TranslationService, private dialogRef : MatDialog,) {
    this.loadCompanyOpenHours();
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

  public openTimeModificationDialog(val:number){
    let dayIndex = this.companyOpenHours.findIndex(x => x.dayOfWeek == val);
    let day = this.companyOpenHours[dayIndex];
    if(!day){
      console.log("undefind")
    }else{
      const dialog = this.dialogRef.open(EditScheduleTimeDialogComponent, {
        width: "550px",
        data: day
      });
      dialog.afterClosed().subscribe((res:EditScheduleTimeDialogResult) => {
        switch (res.status){
            case 'close':
              console.log("0" + val)
              this.companyOpenHours.splice(dayIndex, 1)
              break;
            case 'update':
              if(res.value) {
                this.companyOpenHours[dayIndex] = res.value;
              }
              break
            case 'insert':
              break;
            case 'none':
              break;
        }
      })
    }
  }

  private loadCompanyOpenHours(){
    this.http.get<CompanyOpenHours[]>(`/api/CompanyOpenHours?companyId=${this.auth.getUserFromLocalStorage()?.companyId}`).subscribe(res => {
      this.companyOpenHours = res;
      console.log(this.companyOpenHours);
    }, er => {
      console.log(er)      
    })
  }

}
