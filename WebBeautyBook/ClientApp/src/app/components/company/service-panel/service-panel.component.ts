import {Component, OnInit} from '@angular/core';
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
export class ServicePanelComponent implements OnInit{

  public services: Service[] = [];// Holds a list of services.

  constructor(private auth: AuthService, private dialogRef : MatDialog, private http: HttpClient){
  }

  // Initialize the component and load services when it's ready.
  public async ngOnInit() {
    await this.loadServices();
  }

  // Opens the service update form dialog.
  public async updateServiceForm(item: Service){
    const dialogData = new ServiceFormDialogData();
    dialogData.isUpdateMode = true;
    dialogData.value = item;
    const serviceForm = this.dialogRef.open(ServiceFormComponent, {
      width: "500px",
      data: dialogData
    });
    serviceForm.afterClosed().subscribe(async result => {
      const data = result as ServiceFormDialogResult;
      if (!data.isSuccess || data.result == null) return;
      const i = this.services.findIndex(x => x.id == data.result?.id);
      if (i != -1) {
        this.services[i] = data.result;
      } else {
        await this.loadServices()
        console.error("unexpected error");
      }
    });
  }

  // Opens the service creation form dialog.
  public createServiceForm(){
    const serviceForm = this.dialogRef.open(ServiceFormComponent, {
      width: "500px",
      data: new ServiceFormDialogData()
    });
    serviceForm.afterClosed().subscribe(result => {
      const data = result as ServiceFormDialogResult;
      if(data.isSuccess && data.result != null){
        this.services.push(data.result);
      }
    });
  }

  // TODO: Implement service deletion.
  public deleteService(item: Service){
    alert("TODO")
  }

  // Opens the worker service form dialog for a specific service.
  public workerServiceFrom(item: Service){
    this.dialogRef.open(WorkerServiceFormComponent, {
      data: { serviceId: item.id },
      width: '400px',
    });
  }

  // Load the list of services for the current company.
  private async loadServices(){
    this.services = [];
    const companyId = this.auth.getLocalUserDate()?.companyId;
    try{
      const res = await this.http.get<Service[]>(`api/Service/getServicesForCompany/${companyId}`, {
        headers: this.auth.getHeadersWithToken()
      }).toPromise()
      if(!res){
        this.services = []
      }else this.services = res;
    }catch(e:any){
      alert("Request error, see console for details.")
      console.error(e);
    }
  }

}
