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

  // Input properties for the component
  @Input() styleContainer : "container-fluid" | "container" = "container"
  @Input() defaultRole: string = 'client';
  @Input() redirectSuccess = true;
  @Input() header:string = "Create An Account";
  @Input() showLinkToLogin: boolean = true;

  // Event emitter to send registration results
  @Output() resultEmitter = new EventEmitter<UserDataModel>();

  public mForm: FormGroup;// Form for user input
  public lastErrorMessage: string = "";

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private router: Router) {
    // Initialize the form with validation
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

  public ngOnInit(): void {
    // Set the default role if provided
    if(this.defaultRole != '') this.mForm.controls['role'].setValue(this.defaultRole);
  }

  public onSubmit() {
    // Determine which type of registration to perform based on the default role
    if(this.defaultRole == "own_company") this.registerOwnCompany();
    else if(this.defaultRole == "manager" || this.defaultRole == "worker") this.registerWorker();
    else this.registerNormalPeople();
  }

  private registerNormalPeople(){
    // Handle the case when the form is invalid
    if (!this.mForm.valid) return;
    // Register a regular user
    this.auth.register(this.mForm.value).toPromise().then(result => {
      // Handle successful registration
      this.mForm.reset();
      if(this.redirectSuccess) this.router.navigate(["login"]);
    }).catch(error => {// Handle registration error
      console.error(error);
      this.lastErrorMessage = error.error;
    })
  }

  private registerOwnCompany(){
    let request = this.auth.registerOwnCompany(this.mForm.value);
    if(request == null) alert("What are you doing here?");// Handle the case when the request cannot be made
    else{// Register a company owner
      request.toPromise().then(result => {
        this.resultEmitter.emit(result);// Handle successful registration
        this.mForm.reset()
      }).catch(error => {
        this.lastErrorMessage = error.error;
      });
    }
  }

  private registerWorker(){
    const request = this.auth.registerWorker(this.mForm.value);
    if(!request) alert("What are you doing here?");// Handle the case when the request cannot be made
    else{
      request.toPromise().then(result => {
        this.resultEmitter.emit(result);// Handle successful registration
        this.mForm.reset()
      }).catch(error => this.lastErrorMessage = error.error)
    }
  }

}
