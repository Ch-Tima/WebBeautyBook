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

  constructor(private auth: AuthService, private http : HttpClient, @Inject('BASE_URL') private baseUrl: string, private dialg: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data : CategoryFormDialogData) {
    this.mForm = new FormGroup({
      id: new FormControl(''),
      name: new FormControl(data.value?.name, [Validators.required, Validators.minLength(3)]),
      categoryId: new FormControl(data.value?.categoryId, [Validators.minLength(10)])
    });
  }

  public onSubmit(){
    if(this.mForm.valid){
      if(this.data.isUpdateMode){
        this.update();
      }else{
        this.insert();
      }
    }else{
      console.log("TODO: processing Validators");
    }
  }

  public onCancel(){

  }

  private insert(){
    this.http.put<any>(this.baseUrl + "api/Category", this.mForm.value,
    {
      headers: this.auth.getHeadersWithToken()
    })
    .subscribe(result => {
      var newCategory = new Category();
      newCategory.id = result['id'];
      newCategory.name = this.mForm.controls['name'].getRawValue();
      newCategory.categoryId = this.mForm.controls['categoryId'].getRawValue();

      this.mForm.reset();

      var insertRelut = new CategoryFormDialogResult();
      insertRelut.isSuccess = true;
      insertRelut.reason = "insert";
      insertRelut.result = newCategory

      this.dialg.close(insertRelut);

    }, error => {
      console.log(error);
      this.errorMessages = error.error;
    });

  }

  private update(){
    this.mForm.controls['id'].setValue(this.data.value?.id);
    this.http.post<any>(this.baseUrl + "api/Category", this.mForm.value,
      {
        headers: this.auth.getHeadersWithToken()
      })
      .subscribe(result => {

        this.mForm.reset();

        var updateRelut = new CategoryFormDialogResult();
        updateRelut.isSuccess = true;
        updateRelut.reason = "insert";
        updateRelut.result = this.mForm.value;

        this.dialg.close(updateRelut);

      }, error => {
        console.log(error);
        this.errorMessages = error.error;
      });
  }

}


export class CategoryFormDialogData{
  isUpdateMode: boolean = true;
  value: Category|null = null;
}
export class CategoryFormDialogResult{
  reason: string = ""
  isSuccess: boolean = true
  result: Category|null = null
}
