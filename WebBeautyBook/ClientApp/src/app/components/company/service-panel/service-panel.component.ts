import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Service } from 'src/app/models/Service';
import { ServiceFormComponent, ServiceFormDialogData, ServiceFormDialogResult } from '../service-form/service-form.component';
import { HttpClient } from '@angular/common/http';
import { WorkerServiceFormComponent } from '../worker-service-form/worker-service-form.component';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-service-panel',
  templateUrl: './service-panel.component.html',
  styleUrls: ['./service-panel.component.css']
})
export class ServicePanelComponent {

  public services: Service[] = [];

  constructor(private auth: AuthService, @Inject('BASE_URL') private baseUrl: string, private dialogRef : MatDialog, private http: HttpClient){
    this.loadServices();
  }

  public updateServiceForm(item: Service){
    var dialogData = new ServiceFormDialogData();
    dialogData.isUpdateMode = true;
    dialogData.value = item;
    const serviceForm = this.dialogRef.open(ServiceFormComponent, {
      width: "500px",
      data: dialogData
    });
    serviceForm.afterClosed().subscribe(result => {
      var data = result as ServiceFormDialogResult;

      if(!data.isSuccess || data.result == null) return;

      var i = this.services.findIndex(x => x.id == data.result?.id);

      if(i != -1 && data.result != null){
        this.services[i] = data.result;
      }else{
        this.loadServices()
        console.log("unexpected error");
      }
    });
  }

  public createServiceForm(){
    const serviceForm = this.dialogRef.open(ServiceFormComponent, {
      width: "500px",
      data: new ServiceFormDialogData()
    });
    serviceForm.afterClosed().subscribe(result => {
      var data = result as ServiceFormDialogResult;
      if(data.isSuccess && data.result != null){
        this.services.push(data.result);
      }
    });
  }

  public deleteService(item: Service){
    alert("TODO")
  }

  public workerServiceFrom(item: Service){
    this.dialogRef.open(WorkerServiceFormComponent, {
      data: {
        serviceId: item.id
      },
      width: '400px',
    });
  }

  private loadServices(){
    this.services = [];
    var companyId = this.auth.getLocalUserDate()?.companyId;
    this.http.get<Service[]>(this.baseUrl + `api/Service/getServicesForCompany/${companyId}`, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      result => {
        this.services = result;
      }, error => {
        console.log(error);
        alert("Error -> ServicePanelComponent -> loadServices")
      }
    )
  }

}
