import {HttpClient} from '@angular/common/http';
import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {MatDialog} from '@angular/material/dialog';
import {CategoryFormComponent, CategoryFormDialogData, CategoryFormDialogResult} from '../category-form/category-form.component';
import {Category} from "../../../models/Category";
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-categoty-panel',
  templateUrl: './categoty-panel.component.html',
  styleUrls: ['./categoty-panel.component.css']
})
export class CategotyPanelComponent {

  public list: Category[] = [];
  public mForm: FormGroup;

  constructor(private http: HttpClient, private auth: AuthService, private dialog: MatDialog) {
    // Initialize the form group with form controls and validators
    this.mForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2)]),
      categoryId: new FormControl('', [Validators.minLength(10)])
    });
    this.loadCategories();// Load categories when the component is initialized
  }

  // Send a DELETE request to remove the category
  public onDelete(index: number) {
    const idCategory = this.list[index];
    if (idCategory) {
      console.log("onDelete(" + index + ") -> send");
      this.http.delete("api/Category?id=" + idCategory.id, {
        headers: this.auth.getHeadersWithToken()
      }).subscribe(result => this.loadCategories(), error => console.log(error));
    } else {
      console.log("onDelete() -> Not found item: " + index);
    }
  }

  // Update a category at a specific index
  public onUpdate(index: number) {
    const item = this.list[index];
    if (item == null) {
      alert("index is not exist");
      return;
    }
    // Prepare data for the category form dialog
    const dialogData = new CategoryFormDialogData();
    dialogData.isUpdateMode = true;
    dialogData.value = item;
    // Open the category form dialog
    const dialogRef = this.dialog.open(CategoryFormComponent, {data: dialogData});
    dialogRef.afterClosed().subscribe(result => {
      if (result == undefined) return;
      let data = (result as CategoryFormDialogResult);
      if (data.isSuccess && data.result != null) {
        let item = this.list.find(x => x.id == data.result?.id)
        if (item != undefined) {
          item.name = data.result.name;
          item.categoryId = data.result.categoryId;
        } else {
          this.loadCategories();
          console.log("Something went wrong!");
        }
      } else alert("What?")
    });
  }

  // Insert a new category
  public onInsert() {
    // Prepare data for the category form dialog
    const dialogData = new CategoryFormDialogData();
    dialogData.isUpdateMode = false;
    // Open the category form dialog
    const dialogRef = this.dialog.open(CategoryFormComponent, {data: dialogData});
    dialogRef.afterClosed().subscribe(result => {
      if (result == undefined) return;
      const data = (result as CategoryFormDialogResult);
      if (data.isSuccess && data.result != null) {
        this.list.push(data.result);
      } else {
        alert("What?")
      }
    });
  }

  // Send a GET request to load categories from the server
  private loadCategories() {
    this.http.get<Category[]>("api/Category")
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
