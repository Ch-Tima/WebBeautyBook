import {HttpClient} from '@angular/common/http';
import {Component, Inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Category} from 'src/app/models/Category';
import {Service} from 'src/app/models/Service';
import * as $ from "jquery";
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-service-form',
  templateUrl: './service-form.component.html',
  styleUrls: ['./service-form.component.css']
})
export class ServiceFormComponent implements OnInit {

  public mForm: FormGroup = new FormGroup({}); // Form group for service data.
  public categories: Category[] = []; // Holds a list of categories.
  public errorMessage: string = ""; // Holds error messages.
  public header: string = "Create"; // Header text for the form.
  public isFirstInitFor: boolean = true; // Flag to track if it's the first initialization for event handling.
  public time: any; // Variable to store time information.

  constructor(private auth: AuthService, private http: HttpClient, private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data: ServiceFormDialogData) {
    //Override close for sending results
    this.dialog.backdropClick().subscribe(() => this.dialog.close(new ServiceFormDialogResult()))
    // Initialize form controls
    this.mForm = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.maxLength(100), Validators.minLength(3)]),
      description: new FormControl("", [Validators.maxLength(500)]),
      time: new FormControl("00:06", [Validators.required]),
      price: new FormControl(1.0, [Validators.required, Validators.min(1), Validators.max(50000)]),
      categoryId: new FormControl("", [Validators.required, Validators.maxLength(100), Validators.minLength(3)])
    })
    if (data?.isUpdateMode && data.value) {
      this.header = "Update"
      this.mForm.setValue({
        name: data.value.name,
        description: data.value.description,
        time: data.value.time.length > 6 ? data.value.time.substring(0, data.value.time.lastIndexOf(":")) : data.value.time,
        price: data.value.price,
        categoryId: data.value.categoryId
      });
    }
  }

  public async ngOnInit() {
    await this.loadCategories()//load Categories
  }

  public async onSubmit() {
    if (!this.mForm.valid) return;
    this.errorMessage = '';
    this.data.isUpdateMode ? await this.updateService() : await this.createService();
  }

  public setSubCategory(event: any) {
    const id = event.target.value;//get category Id from "option" value
    let item = this.categories.find(x => x.id == id)
    if (item == undefined) {//if not found in categories, then it is a subcategory
      this.mForm.controls["categoryId"].setValue(id);
    } else {
      if (item.categories.length > 0) {//if "item.categories" has a subcategory
        this.mForm.controls["categoryId"].setValue("");
        $("#subCategory").prop("hidden", false);//show "select" subCategory
        $(`#subCategory`).empty();
        $("#subCategory").append("<option selected disabled>non</option>")
        item.categories.forEach(item => $(`#subCategory`).append(`<option value="${item.id}">${item.name}</option>`))
      } else {//set category id if there is no subcategory
        this.mForm.controls["categoryId"].setValue(item.id);
        $("#subCategory").prop("hidden", true);
      }
    }
  }

  // Create a new service.
  private async createService() {
    try {
      //send to api "api/Service"
      const result = await this.http.put<Service>("api/Service", this.mForm.value, {
        headers: this.auth.getHeadersWithToken()
      }).toPromise();
      if (result !== undefined) {
        const data = new ServiceFormDialogResult(); //preparation result
        data.isSuccess = true;
        data.result = result;
        this.dialog.close(data); //close dialog and send result
      } else console.error("Result is undefined");
    }catch (error:any) {
      this.showError(error);
    }

  }

  // Update an existing service.
  private async updateService() {
    try {
      await this.http.post(`api/Service?Id=${this.data.value?.id}`, this.mForm.value, {
        headers: this.auth.getHeadersWithToken()
      }).toPromise().catch();
      const data = new ServiceFormDialogResult(); //preparation result
      data.isSuccess = true;
      this.mForm.addControl("id", new FormControl(this.data.value?.id))
      data.result = this.mForm.value
      this.dialog.close(data); //close dialog and send result
    } catch (error: any) {
      this.showError(error);
    }
  }

  /**
   * - Event only works when isUpdateMode: true, to initialize categories and subcategories in "selection".
   * - Initialization happens once thanks to: isFirstInitFor
   */
  public onFinishFor() {
    if (this.data.value != null && this.data.isUpdateMode && this.isFirstInitFor) {
      let main = this.categories.find(x => x.id == this.data.value?.categoryId);
      if (main != undefined) {
        $(`#mainCategory option[value='${main.id}']`).prop('selected', true);
      } else {
        let mainWithSub = this.categories.find(x => x.categories?.find(y => y.id == this.data.value?.categoryId));
        if (mainWithSub != undefined) {
          $(`#mainCategory option[value='${mainWithSub.id}']`).prop('selected', true);
          let sub = mainWithSub.categories.find(x => x.id == this.data.value?.categoryId);
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

  // Load the list of categories
  private async loadCategories() {
    try {
      const result = await this.http.get<Category[]>("api/Category/GetMainCategories").toPromise();
      this.categories = result || [];
    } catch (error) {
      console.log(error);
    }
  }

  private showError(error: any){
    console.error(error);
    if (error.error.errors != undefined) { //error from model
      this.errorMessage = Object.values<any>(error.error.errors)[0][0];
    } else { //error from controller
      this.errorMessage = error.error;
    }
  }

}

export class ServiceFormDialogData {
  isUpdateMode: boolean = false;
  value: Service | null = null;
}

export class ServiceFormDialogResult {
  public isSuccess: boolean = false;
  public result: Service | null = null;
}
