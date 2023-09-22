import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserDataModel } from 'src/app/models/UserDataModel';
import { CompareValidator } from 'src/app/validators/CompareValidator';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {

  @Input() styleContainer : "container-fluid" | "container" = "container"
  @Input() defaultRole: string = 'client';
  @Input() redirectSuccess = true;
  @Input() header:string = "Create An Account";
  @Input() showLinkToLogin: boolean = true;

  @Output() resultEmitter = new EventEmitter<UserDataModel>();

  mForm: FormGroup = new FormGroup({});
  lastErrorMessage: string = "";

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private router: Router)
  {
    this.mForm = formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email]),
      userName: new FormControl('', [Validators.required, Validators.maxLength(100), Validators.minLength(4)]),
      userSurname: new FormControl('', [Validators.required, Validators.maxLength(100), Validators.minLength(4)]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)]),
      confirmPassword: new FormControl('', [Validators.required]),
      role: new FormControl(this.defaultRole, [Validators.required])
    }, {
      validator: CompareValidator("password", "confirmPassword")
    });
  }


  ngOnInit(): void {
    if(this.defaultRole != '') this.mForm.controls['role'].setValue(this.defaultRole);
  }

  onSubmit() {
    if(this.defaultRole == "own_company") this.registerOwnCompany();
    else if(this.defaultRole == "manager" || this.defaultRole == "worker") this.registerWorker();
    else this.registerNormalPeople();
  }

  private registerNormalPeople(){
    if (!this.mForm.valid) {
      console.log("Error!");
      console.log(this.mForm);
    } else {
      this.auth.register(this.mForm.value)
        .subscribe(result => {
          console.log(result);
          this.mForm.reset();
          if(this.redirectSuccess) this.router.navigate(["login"]);
        }, error => {
          console.log(error);
          this.lastErrorMessage = error.error;
        });
    }
  }

  private registerOwnCompany(){
    var requst = this.auth.registerOwnCompany(this.mForm.value);
    if(requst == null) alert("What are you doing here?");
    else{
      requst.subscribe(
        result => {
          this.resultEmitter.emit(result);
          this.mForm.reset();
        }, error => {
          this.lastErrorMessage = error.error;
        }
      );
    }
  }

  private registerWorker(){
    var requst = this.auth.registerWorker(this.mForm.value);
    if(requst == null) alert("What are you doing here?");
    else{
      requst.subscribe(
        result => {
          this.resultEmitter.emit(result);
          this.mForm.reset();
        }, error => {
          this.lastErrorMessage = error.error;
        }
      );
    }
  }

}
