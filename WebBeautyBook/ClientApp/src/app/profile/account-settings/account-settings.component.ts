import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserDataModel } from 'src/app/models/UserDataModel';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.css']
})
export class AccountSettingsComponent {

  public isChangeFrom:boolean = false;
  public error:string = '';
  public user$:Observable<UserDataModel|undefined> = this.getUserData();
  public mForm: FormGroup;
  
  public url: string|ArrayBuffer|null|undefined = null; //preview image selected by the user
  private fileToUpload:File|undefined; //the file selected by the user

  constructor(private auth: AuthService, private http: HttpClient){
    //Init form
    this.mForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(100), Validators.minLength(4)]),
      surname: new FormControl('', [Validators.required, Validators.maxLength(100), Validators.minLength(4)]),
      phoneNumber: new FormControl('', [Validators.pattern('[+0-9]{12}')]),
      file: new FormControl(null),
      photo: new FormControl(null),
    });
  }

  public async submit(){
    if(!this.mForm.valid)
    {
      console.log("form is not valid");
      return;
    }

    //wrap data in FormData
    var formData = new FormData();
    if(this.fileToUpload != undefined){
      formData.append('file', this.fileToUpload, this.fileToUpload.name);
    }
    formData.append('name', this.mForm.controls['name'].getRawValue())
    formData.append('surname', this.mForm.controls['surname'].getRawValue())
    formData.append('phoneNumber', this.mForm.controls['phoneNumber'].getRawValue() ?? '')

    //send
    await this.http.post<UserDataModel>("api/User", formData, {
      headers: this.auth.getHeadersWithToken(),
    }).toPromise().then(result => {
      this.user$.subscribe(user => {
        if(result != undefined) {
          user = result;//update form user data
          this.auth.saveUserData(result);//update local user data "in localStorage"
          this.isChangeFrom = false;//hide button
          alert("Ok");//TODO replace with beautiful push notifications
        }else{
          alert("What!?")
        }
      })
    }).catch(error => {
      //If there is an error in the model, then a list of errors will come
      if(error.error.errors != undefined && Object.values(error.error.errors)[0] != undefined){
        this.error = Object.values(error.error.errors)[0] + "";
      }else{//Just an error line
        this.error = error.error;
      }
    });
  }

  public changePassword(){
    var email = this.auth.getLocalUserDate()?.email;
    if(email != undefined && email.length > 0){
      //request a password reset
      this.auth.ForgotPassword(email).subscribe(result => {
        alert("Check your email and follow the instructions to reset your password and sign in again.");
      }, error => {
        alert(error.error)
      });
    }
  }

  public onFileChange(event:any) {
    if (event.target.files && event.target.files[0]){
      const reader = new FileReader();
    
      this.fileToUpload = event.target.files[0];
      reader.readAsDataURL(event.target.files[0]);
      reader.onloadend = (e) => {
        this.url = e.target?.result;
      };

      this.isChangeFrom = true; //show button
    }
  }

  //just load user data from server
  private getUserData(){
    return this.auth.getUserData().pipe(tap(user => { 
      this.mForm.patchValue(user)
      this.mForm.valueChanges.subscribe(x => this.isChangeFrom = true)
    }))
  }

}
