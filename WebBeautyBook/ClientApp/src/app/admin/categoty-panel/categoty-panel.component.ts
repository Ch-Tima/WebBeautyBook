import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Category } from "../../models/Category";
import { AuthService } from 'src/app/services/auth.service';
import {MatDialog} from '@angular/material/dialog';
import { CategoryFormComponent, CategoryFormDialogData, CategoryFormDialogResult } from '../category-form/category-form.component';

@Component({
  selector: 'app-categoty-panel',
  templateUrl: './categoty-panel.component.html',
  styleUrls: ['./categoty-panel.component.css']
})
export class CategotyPanelComponent {

  public list: Category[] = [];
  public mForm: FormGroup;

  constructor(private http : HttpClient, @Inject('BASE_URL') private baseUrl: string, private auth: AuthService, private dialog: MatDialog) {
    this.mForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2)]),
      categoryId: new FormControl('', [Validators.minLength(10)])
    });
    this.loadCategories();
  }

  public onDelete(index: number){
    var idCategory = this.list[index];
    if(idCategory != null && idCategory != undefined){
      console.log("onDelete(" + index + ") -> send");
      this.http.delete(this.baseUrl + "api/Category?id=" + idCategory.id, {
        headers: this.auth.getHeadersWithToken()
      })
      .subscribe(result => {
        this.loadCategories();
      }, error => {
          console.log(error);
      });

    }else{
      console.log("onDelete() -> Not found item: " + index);
    }
  }
  
  public onUpdate(index: number){
    var item = this.list[index];
    if(item == null || item == undefined)
    {
      alert("index is not exist");
      return;
    }

    var dialgData = new CategoryFormDialogData();
    dialgData.isUpdateMode = true;
    dialgData.value = item;

    const dialogRef = this.dialog.open(CategoryFormComponent, { data: dialgData });
    dialogRef.afterClosed().subscribe(result => {
      if(result == undefined) return;

       var data = (result as CategoryFormDialogResult);
       if(data.isSuccess && data.result != null){
        var item = this.list.find(x => x.id == data.result?.id)

        if(item != undefined){
          item.name = data.result.name;
          item.categoryId = data.result.categoryId;
        }else{
          this.loadCategories();
          console.log("Something went wrong!");
        }
       }else{
        alert("What?")
       }
    });
  }

  public onInsert(){
    var dialgData = new CategoryFormDialogData();
    dialgData.isUpdateMode = false;

    const dialogRef = this.dialog.open(CategoryFormComponent, { data: dialgData });

    dialogRef.afterClosed().subscribe(result =>  {
      if(result == undefined) return;

       var data = (result as CategoryFormDialogResult);
       if(data.isSuccess && data.result != null){
        this.list.push(data.result);
       }else{
        alert("What?")
       }
    });
  }


  private loadCategories(){
    this.http.get<Category[]>(this.baseUrl + "api/Category")
    .subscribe(
      result => {
          this.list = result;
      }, error => {
        console.log("Error -> Home -> loadCategories()")
        console.log(error);
      }
    );
  }

}
