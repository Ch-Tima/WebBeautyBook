import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Category } from 'src/app/models/Category';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.css']
})
export class CategoryFormComponent {

  public mForm: FormGroup;
  public header: string = "CategoryFormComponent";
  public errorMessages : string = "";

  constructor(private auth: AuthService, private http : HttpClient, private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : CategoryFormDialogData) {
    // Create a form group with form controls and initial values
    this.mForm = new FormGroup({
      id: new FormControl(''),
      name: new FormControl(data.value?.name, [Validators.required, Validators.minLength(3)]),
      categoryId: new FormControl(data.value?.categoryId, [Validators.minLength(10)])
    });
  }

  public onSubmit(){
    if(this.mForm.valid){
      if(this.data.isUpdateMode) this.update();
      else this.insert();
    }else{
      console.log("TODO: processing Validators");//TODO: processing Validators
    }
  }

  public onCancel(){}

  private insert(){
    // Send a PUT request to insert a new category
    this.http.put<any>("api/Category", this.mForm.value,
    {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      // Create a new category object with the result data
      let newCategory = new Category();
      newCategory.id = result['id'];
      newCategory.name = this.mForm.controls['name'].getRawValue();
      newCategory.categoryId = this.mForm.controls['categoryId'].getRawValue();
      // Reset the form
      this.mForm.reset();
      // Create a dialog result with success information
      let insert = new CategoryFormDialogResult();
      insert.isSuccess = true;
      insert.reason = "insert";
      insert.result = newCategory
      // Close the dialog with the result
      this.dialog.close(insert);
    }, error => {
      console.log(error);
      this.errorMessages = error.error;
    });
  }

  private update(){
    this.mForm.controls['id'].setValue(this.data.value?.id);
    this.http.post<any>("api/Category", this.mForm.value, {
        headers: this.auth.getHeadersWithToken()
      })
      .subscribe(result => {
        this.mForm.reset();// Reset the form
        // Create a dialog result with success information
        const updateResult = new CategoryFormDialogResult();
        updateResult.isSuccess = true;
        updateResult.reason = "insert";
        updateResult.result = this.mForm.value;
        // Close the dialog with the result
        this.dialog.close(updateResult);
      }, error => {
        console.log(error);
        this.errorMessages = error.error;
      });
  }

}
// Data class for the dialog
export class CategoryFormDialogData{
  isUpdateMode: boolean = true;
  value: Category|null = null;
}
// Result class for the dialog
export class CategoryFormDialogResult{
  reason: string = ""
  isSuccess: boolean = true
  result: Category|null = null
}
