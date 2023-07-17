import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Category } from "../../models/Category";
import { AuthService } from 'src/app/services/auth.service';
// import {MatDialog, MatDialogModule} from '@angular/material/dialog';
// import {MatButtonModule} from '@angular/material/button';

@Component({
  selector: 'app-categoty-panel',
  templateUrl: './categoty-panel.component.html',
  styleUrls: ['./categoty-panel.component.css']
})
export class CategotyPanelComponent {

  public list: Category[] = [];
  public mForm: FormGroup;
//, private dialog: MatDialog
  constructor(private http : HttpClient, @Inject('BASE_URL') private baseUrl: string, private auth: AuthService) {
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
        console.log(result);
        this.list.splice(index, 1);
      }, error => {
          console.log(error);
      });

    }else{
      console.log("onDelete() -> Not found item: " + index);
    }
  }
  
  public onUpdate(index: number){
    
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
