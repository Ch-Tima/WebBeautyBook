import { HttpClient } from '@angular/common/http';
import { Component, Inject} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Category } from 'src/app/models/Category';
import { Service } from 'src/app/models/Service';
import * as $ from "jquery";
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-service-form',
  templateUrl: './service-form.component.html',
  styleUrls: ['./service-form.component.css']
})
export class ServiceFormComponent{

  public mForm: FormGroup = new FormGroup({})
  public categories: Category[] = []
  public errorMessage: string = "";
  public header: string = "Create";
  public isFirstInitFor: boolean = true;
  public time:any;

  constructor(private auth: AuthService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string,
  private dialg: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : ServiceFormDialogData){
    //Override close for sending results
    this.dialg.backdropClick().subscribe(() => {
      this.dialg.close(new ServiceFormDialogResult());
    })

    //Init form
    this.mForm = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.maxLength(100), Validators.minLength(3)]),
      description: new FormControl("", [Validators.maxLength(500)]),
      time: new FormControl("00:06", [Validators.required]),
      price: new FormControl(1.0, [Validators.required, Validators.min(1), Validators.max(50000)]),
      categoryId: new FormControl("", [Validators.required, Validators.maxLength(100), Validators.minLength(3)])
    })

    if(data != null && data != undefined && data.isUpdateMode && data.value != null){
      this.header = "Update"
      this.mForm.setValue({
        name: data.value.name,
        description: data.value.description,
        time: data.value.time.length > 6 ? data.value.time.substring(0, data.value.time.lastIndexOf(":")) : data.value.time,
        price: data.value.price,
        categoryId: data.value.categoryId
      });
    }
    //load Categories
    this.loadCategories();
  }

  public onSubmit(){
    if(!this.mForm.valid) return;

    this.errorMessage = '';

    if(this.data.isUpdateMode){
      this.updateService();
    }else{
      this.createService();
    }
  }

  public setSubCategory(event:any){
    var id = event.target.value;//get category Id from "option" value
    var item = this.categories.find(x => x.id == id)
    if(item == undefined){//if not found in categories, then it is a subcategory
      this.mForm.controls["categoryId"].setValue(id);
    }else{
      if(item.categories.length > 0){//if "item.categories" has a subcategory
        this.mForm.controls["categoryId"].setValue("");

        $("#subCategory").prop("hidden", false);//show "select" subCategory

        $(`#subCategory`).empty();
        $("#subCategory").append("<option selected disabled>nono</option>")
        item.categories.forEach(item => {
          $(`#subCategory`).append(`<option value="${item.id}">${item.name}</option>`)
        })

      }else{//set category id if there is no subcategory
        this.mForm.controls["categoryId"].setValue(item.id);
        $("#subCategory").prop("hidden", true);
      }
    }
  }

  private createService(){
    //send to api "api/Service"
    this.http.put<Service>(this.baseUrl + "api/Service", this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      var data = new ServiceFormDialogResult(); //preparation result
      data.isSuccess = true;
      data.result = result;
      this.dialg.close(data); //close dialog and send result
    }, error => {
      console.log(error);
      if(error.error.errors != undefined){ //erorr from model
        this.errorMessage = Object.values<any>(error.error.errors)[0][0];
      }else{ //error from controller
        this.errorMessage = error.error;
      }
    });

  }

  private updateService(){
    this.http.post(this.baseUrl + `api/Service?Id=${this.data.value?.id}`, this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      var data = new ServiceFormDialogResult(); //preparation result
      data.isSuccess = true;
      this.mForm.addControl("id", new FormControl(this.data.value?.id))
      data.result = this.mForm.value
      this.dialg.close(data); //close dialog and send result
    }, error => {
      console.log(error);
      if(error.error.errors != undefined){ //erorr from model
        this.errorMessage = Object.values<any>(error.error.errors)[0][0];
      }else{ //error from controller
        this.errorMessage = error.error;
      }
    })
  }

  /**
   * - Event only works when isUpdateMode: true, to initialize categories and subcategories in "selection".
   * - Initialization happens once thanks to: isFirstInitFor
   */
  public onFinishFor() {
    if(this.data.value != null && this.data.isUpdateMode && this.isFirstInitFor){
      var main = this.categories.find(x => x.id == this.data.value?.categoryId);
      if(main != undefined){
       $(`#mainCategory option[value='${main.id}']`).prop('selected', true);
      }else{
        var mainWithSub = this.categories.find(x => x.categories?.find(y => y.id == this.data.value?.categoryId));

        if(mainWithSub != undefined){
          $(`#mainCategory option[value='${mainWithSub.id}']`).prop('selected', true);
          var sub = mainWithSub.categories.find(x => x.id == this.data.value?.categoryId);
          mainWithSub.categories.forEach(item => {
            $(`#subCategory`).append(`<option value="${item.id}">${item.name}</option>`)
          })
          $(`#subCategory option[value='${sub?.id}']`).prop('selected', true);
          $(`#subCategory`).prop("hidden", false);
        }
      }
      this.isFirstInitFor = false;
    }
  }

  private loadCategories(){
    this.http.get<Category[]>(this.baseUrl + "api/Category/GetMainCategories").subscribe(
      result => {
        this.categories = result;
      }, error => {
        console.log(error);
      }
    );
  }
}

export class ServiceFormDialogData{
  isUpdateMode: boolean = false;
  value: Service|null = null;
}

export class ServiceFormDialogResult{

  public isSuccess: boolean = false;
  public result: Service|null = null;

}
