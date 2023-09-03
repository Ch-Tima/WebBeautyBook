import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Appointment } from 'src/app/models/Appointment';
import { Worker } from 'src/app/models/Worker';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-appointment-dialog',
  templateUrl: './appointment-dialog.component.html',
  styleUrls: ['./appointment-dialog.component.css']
})
export class AppointmentDialogComponent implements OnInit{

  public mForm:FormGroup;

  public error: string|undefined;
  public msgForAvailableTime:string|undefined;

  public workers: Worker[] = [];
  public availableTime:string[] = [];

  constructor(private auth: AuthService, private http: HttpClient, private formBuilder: FormBuilder, private dialg: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : AppointmentDialogDate){
    //Override close for sending results
    this.dialg.backdropClick().subscribe(() => this.dialg.close(new AppointmentDialogResult()))

    this.mForm = this.formBuilder.group({
      date: [null, [Validators.required]],
      time: [null, [Validators.required]],
      note: ['', [Validators.maxLength(500)]],
      workerId: [null, [Validators.required]],
      serviceId: [this.data.serviceId, [Validators.required]],
    });
  }

  public async ngOnInit() {
    if(!this.data.isUpdateMode && this.data.serviceId != undefined){
      var worker = await this.loadServiceDate(this.data.serviceId);
      if(worker == undefined || worker.length == 0){
        this.dialg.close();
        return;
      }
      this.workers = worker;
    }
  }
  public onSubmit(){
    if(this.mForm.invalid) return;
    this.error = undefined;
    if(this.data.isUpdateMode ){
      this.update()
    }else{
      this.create();
    }
  }
  
  public async findFreeTime(){

    console.log(this.mForm.value);

    if(!this.mForm.controls.date.valid || !this.mForm.controls.workerId.valid) return;

    try{//get an employee's free time for a service
      var availableTime = await this.http.get<string[]>(`api/Worker/getWorkersFreeTimeForService?workerId=${this.mForm.controls.workerId.getRawValue()}&date=${this.mForm.controls.date.getRawValue().toLocaleDateString()}&serviceId=${this.data.serviceId}`, {
      }).toPromise().catch(e => {
        console.log(e);
        this.msgForAvailableTime = e.error;
      });

      if(availableTime != undefined) {
        this.availableTime = availableTime;
      }

    }catch(e){
      console.log("catch->");
      console.log(e);
    }
  }

  private update(){
    alert("TODO");
  }

  private create(){
    this.http.put<any>('api/Appointment/CreateAppointmentViaClient', this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(val => {
      this.dialg.close({ isSuccess: true, action: 'create', value: val } as AppointmentDialogResult)
    }, e => {
      console.log(e)
      this.error = e.error
    });
  }

  private async loadServiceDate(serviceId:string){
    return await this.http.get<Worker[]>(`api/Worker/getWorkersByServiceId/${serviceId}`, {
      headers: this.auth.getHeadersWithToken()
    }).toPromise().catch(error => {
      console.log(error);
    });
  }

}
export class AppointmentDialogDate{
  isUpdateMode: boolean = false;
  serviceId: string|undefined;
  public value: Appointment|undefined;
}
export class AppointmentDialogResult{
  public isSuccess: boolean = false;
  public action: 'remove'|'update'|'create'|'close' = 'close';
  public value: Appointment|undefined;
}