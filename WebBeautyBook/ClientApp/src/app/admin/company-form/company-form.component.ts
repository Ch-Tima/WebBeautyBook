import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Company } from 'src/app/models/Company';
import { Location } from 'src/app/models/Location';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-company-form',
  templateUrl: './company-form.component.html',
  styleUrls: ['./company-form.component.css']
})
export class CompanyFormComponent {
  
  mForm: FormGroup = new FormGroup({});
  listCountry:Location[] = [];
  listCity:Location[] = [];
  submitted: boolean = false;
  error: string = "";

  @Output() resultEmitter = new EventEmitter<Company>();
  
  constructor(private formBuilder: FormBuilder, private http: HttpClient,  @Inject('BASE_URL') private baseUrl: string, private auth: AuthService) {
    this.mForm = formBuilder.group({
      nameCompany: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
      feedbackEmail: new FormControl('', [Validators.required, Validators.email]),
      emailOwn: new FormControl('', [Validators.required, Validators.email]),
      locationId: new FormControl('', [Validators.required, Validators.minLength(12), Validators.maxLength(100)]),
      address: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(100)])
    }); 
    this.loadCountries();
  }

  public onSubmit(){
    if(!this.mForm.valid){
      alert("Error: form is not valid");
      return;
    }

    this.http.put<any>(this.baseUrl + "api/Company", this.mForm.value, {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(result => {
      this.resultEmitter.emit(result);
    }, error => {
      this.error = error.error;
      console.log(error);
    });
  }

  public loadCityies(_value:any){
    this.http.get<Location[]>(this.baseUrl + "api/Location/getAllCity?contry=" + _value.target.value)
    .subscribe(result => {
      if(result != null) this.listCity = result;
      else alert("Error: API (getAllCity) return null")
    }, error => {
      alert(error);
      console.log(error);
    });
  }

  private loadCountries(){
    this.listCity = [];
    this.mForm.controls["locationId"].setValue('');
    this.http.get<Location[]>(this.baseUrl + "api/Location/getAllCountry")
    .subscribe(result => {
      if(result != null) this.listCountry = result;
      else alert("Error: API (getAllCountry) return null")
    }, error => {
      alert(error);
      console.log(error);
    });
  }

}
