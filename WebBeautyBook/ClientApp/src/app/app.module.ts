import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MatDialogModule } from "@angular/material/dialog"
import { NgxMatTimepickerModule } from "ngx-mat-timepicker"
import { FullCalendarModule } from '@fullcalendar/angular';
import { MatFormFieldModule } from '@angular/material/form-field';
//import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { RegisterPageComponent } from './auth/register-page/register-page.component';
import { LoginPageComponent } from './auth/login-page/login-page.component';
import { EmailConfirmationComponent } from './auth/email-confirmation/email-confirmation.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { AdminPageComponent } from './admin/admin-page/admin-page.component';
import { NavLeftMenuComponent } from './nav-left-menu/nav-left-menu.component';
import { CategotyPanelComponent } from './admin/categoty-panel/categoty-panel.component';
import { CategoryFormComponent } from './admin/category-form/category-form.component';
import { OwnCompanyPanelComponent } from './admin/own-company-panel/own-company-panel.component';
import { CompanyPanelComponent } from './admin/company-panel/company-panel.component';
import { ListCompanyComponent } from './admin/list-company/list-company.component';
import { CompanyFormComponent } from './admin/company-form/company-form.component';
import { MyCompanyPanelComponent } from './company/my-company-panel/my-company-panel.component';
import { AppointmentComponent } from './company/appointment/appointment.component';
import { EmployeesPanelComponent } from './company/employees-panel/employees-panel.component';
import { ServicePanelComponent } from './company/service-panel/service-panel.component';
import { ServiceFormComponent } from './company/service-form/service-form.component';
import { WorkerServiceFormComponent } from './company/worker-service-form/worker-service-form.component';

const routes: Routes = [

  //Public
  { path: '', component: HomeComponent, pathMatch: 'full' },

   //AdminPanel
  { path: "adminPanel", component: AdminPageComponent },
  
  //CompnyPanel
  { path: "companyPanel", component: MyCompanyPanelComponent },

  //Auth
  { path: "login", component: LoginPageComponent},
  { path: "register", component: RegisterPageComponent },
  { path: "forgotPassword", component: ForgotPasswordComponent },
  { path: "resetPassword", component: ResetPasswordComponent },
  { path: "emailConfirmation", component: EmailConfirmationComponent },

];


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RegisterPageComponent,
    LoginPageComponent,
    EmailConfirmationComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    AdminPageComponent,
    NavLeftMenuComponent,
    CategotyPanelComponent,
    CategoryFormComponent,
    OwnCompanyPanelComponent,
    CompanyPanelComponent,
    ListCompanyComponent,
    CompanyFormComponent,
    MyCompanyPanelComponent,
    AppointmentComponent,
    EmployeesPanelComponent,
    ServicePanelComponent,
    ServiceFormComponent,
    WorkerServiceFormComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    MatDialogModule,
    NgxMatTimepickerModule,
    FullCalendarModule,
    MatFormFieldModule,
    //NgxMaterialTimepickerModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
