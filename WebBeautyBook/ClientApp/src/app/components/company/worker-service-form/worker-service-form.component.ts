import { Component, Inject } from '@angular/core';
import { Worker } from "../../../models/Worker"
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-worker-service-form',
  templateUrl: './worker-service-form.component.html',
  styleUrls: ['./worker-service-form.component.css']
})
export class WorkerServiceFormComponent {

  public selectedWorkers: Worker[] = []; // Holds selected workers.
  public workers: Worker[] = []; // Holds all available workers.
  public errorMessage: string | null = ""; // Holds error messages.
  private readonly serviceId: string = ""; // Holds the current service ID.;

  constructor(private dialogRef: MatDialogRef<WorkerServiceFormComponent>, private auth: AuthService,
    @Inject(MAT_DIALOG_DATA) private data : any,  private http: HttpClient) {
    this.serviceId = data.serviceId;
    if(!this.serviceId){
      this.dialogRef.close();
      console.error("serviceId cannot be null or undefined!");
      return;
    }
    //load "Workers" only with this service
    this.loadWorkersToSrvice(this.serviceId);
  }

  public insertWorkerToService(worker: Worker){
    this.http.post("api/Assignment/insertWorkerToService", {
      workerId: worker.id,
      serviceId: this.serviceId
    }, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.workers = this.workers.filter(x => x.id !== worker.id);
        this.selectedWorkers.push(worker);
      }, error => {
        console.log(error);
        this.errorMessage = error.error;
      }
    );
  }

  public removeWorkerFromService(worker: Worker){
    this.http.post("api/Assignment/removeWorkerFromService", {
      workerId: worker.id,
      serviceId: this.serviceId
    }, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.selectedWorkers = this.selectedWorkers.filter(x => x.id !== worker.id);
        this.workers.push(worker);
      }, error => {
        console.log(error);
        this.errorMessage = error.error;
      }
    );
  }

  //load all Workers in Company
  private loadWorkers(){
    this.http.get<Worker[]>("api/Company/getWorkers", {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.workers = result;
        //leave workers without this Service
        this.selectedWorkers.forEach(x => {// Filter out workers who are already associated with this service.
          this.workers = this.workers.filter(w => w.id !== x.id)
        });
      }, error => {
        alert(error);
      }
    )
  }

  //load workers who own this service
  private loadWorkersToSrvice(serviceId: string){
    this.http.get<Worker[]>(`api/Worker/getWorkersByServiceId/${serviceId}`).subscribe(
      result => {
        this.selectedWorkers = result;
        //load all workers
        this.loadWorkers();
      }, error => {
        alert(error);
        console.log("loadWorkersToSrvice");
      }
    );
  }

}
