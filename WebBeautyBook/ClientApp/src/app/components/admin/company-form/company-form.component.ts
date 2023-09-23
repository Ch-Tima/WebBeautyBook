import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Company } from 'src/app/models/Company';
import { Location } from 'src/app/models/Location';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-company-form',
  templateUrl: './company-form.component.html',
  styleUrls: ['./company-form.component.css']
})
export class CompanyFormComponent {

  // Form group for the company input fields
  mForm: FormGroup = new FormGroup({});
  // Arrays to store lists of countries and cities
  listCountry: Location[] = [];
  listCity: Location[] = [];
  // Flag to track whether the form has been submitted
  submitted: boolean = false;
  // Variable to store error messages
  error: string = "";
  // Event emitter to notify the parent component with the result
  @Output() resultEmitter = new EventEmitter<Company>();

  constructor(private formBuilder: FormBuilder, private http: HttpClient, private auth: AuthService) {
    this.mForm = formBuilder.group({
      nameCompany: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
      feedbackEmail: new FormControl('', [Validators.required, Validators.email]),
      emailOwn: new FormControl('', [Validators.required, Validators.email]),
      locationId: new FormControl('', [Validators.required, Validators.minLength(12), Validators.maxLength(100)]),
      address: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(100)])
    });
    this.loadCountries();
  }

  // Handle form submission
  public onSubmit(){
    if(!this.mForm.valid){// Check if the form is valid
      alert("Error: form is not valid");
      return;
    }
    // Send a PUT request to create/update a company
    this.http.put<any>("api/Company", this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      this.resultEmitter.emit(result);
    }, error => {
      this.error = error.error;
      console.log(error);
    });
  }

  // Load cities based on the selected country
  public loadCities(_value:any){
    this.http.get<Location[]>("api/Location/getAllCity?contry=" + _value.target.value)
    .subscribe(result => {
      if(result != null) this.listCity = result;
      else alert("Error: API (getAllCity) return null")
    }, error => {
      alert(error);
      console.log(error);
    });
  }

  // Load the list of countries
  private loadCountries(){
    this.listCity = [];
    this.mForm.controls["locationId"].setValue('');
    this.http.get<Location[]>("api/Location/getAllCountry")
    .subscribe(result => {
      if(result != null) this.listCountry = result;
      else alert("Error: API (getAllCountry) return null")
    }, error => {
      alert(error);
      console.log(error);
    });
  }

}
