import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { BrowserAnimationsModule, provideAnimations } from "@angular/platform-browser/animations";
import { FullCalendarModule } from '@fullcalendar/angular';
import { DragScrollModule } from 'ngx-drag-scroll';
import { NgxMatTimepickerModule } from "ngx-mat-timepicker"
import { ToastrModule, provideToastr } from 'ngx-toastr';
//@angular/material
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from "@angular/material/dialog"
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatRadioModule } from '@angular/material/radio';

//Components
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
import { LocationPanelComponent } from './admin/location-panel/location-panel.component';
import { LocationFormComponent } from './admin/location-form/location-form.component';
import { MyCompanyPanelComponent } from './company/my-company-panel/my-company-panel.component';
import { AppointmentComponent } from './company/appointment/appointment.component';
import { EmployeesPanelComponent } from './company/employees-panel/employees-panel.component';
import { ServicePanelComponent } from './company/service-panel/service-panel.component';
import { ServiceFormComponent } from './company/service-form/service-form.component';
import { WorkerServiceFormComponent } from './company/worker-service-form/worker-service-form.component';
import { InvitationEmployeeComponent } from './company/invitation-employee/invitation-employee.component';
import { AcceptInvitationPageComponent } from './company/accept-invitation-page/accept-invitation-page.component';
import { FooterComponent } from './footer/footer.component';
import { CompanyPageComponent } from './publicCompany/company-page/company-page.component'
import { FilterPipe } from './filterPipes/FilterPipe';
import { ImageSliderComponent } from './image-slider/image-slider.component';
import { ProfilePageComponent } from './profile/profile-page/profile-page.component';
import { AccountSettingsComponent } from './profile/account-settings/account-settings.component';
import { FavouritesComponent } from './profile/favourites/favourites.component';
import { CompanyCardComponent } from './company/company-card/company-card.component';
import { AppointmentDialogComponent } from './publicCompany/appointment-dialog/appointment-dialog.component';
import { ReservationDialogComponent } from './company/reservation-dialog/reservation-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe } from '@angular/common';

const routes: Routes = [

  //Public
  { path: '', component: HomeComponent, pathMatch: 'full' },

   //AdminPanel
  { path: "adminPanel", component: AdminPageComponent },
  
  //CompnyPanel
  { path: "companyPanel", component: MyCompanyPanelComponent },
  { path: "acceptInvitation" , component: AcceptInvitationPageComponent },
  { path: "company", component: CompanyPageComponent },

  //Auth
  { path: "login", component: LoginPageComponent},
  { path: "register", component: RegisterPageComponent },
  { path: "forgotPassword", component: ForgotPasswordComponent },
  { path: "resetPassword", component: ResetPasswordComponent },
  { path: "emailConfirmation", component: EmailConfirmationComponent },

  //Profile
  { path: 'profile', component: ProfilePageComponent },

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
    LocationPanelComponent,
    LocationFormComponent,
    MyCompanyPanelComponent,
    AppointmentComponent,
    EmployeesPanelComponent,
    ServicePanelComponent,
    ServiceFormComponent,
    WorkerServiceFormComponent,
    InvitationEmployeeComponent,
    AcceptInvitationPageComponent,
    FooterComponent,
    CompanyPageComponent,
    FilterPipe,
    ImageSliderComponent,
    ProfilePageComponent,
    AccountSettingsComponent,
    FavouritesComponent,
    CompanyCardComponent,
    AppointmentDialogComponent,
    ReservationDialogComponent,
  ],
  imports: [
    BrowserModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    MatDialogModule,
    NgxMatTimepickerModule,
    FullCalendarModule,
    DragScrollModule,
    ToastrModule.forRoot(),
    //mat for timepicker and datepicker
    MatFormFieldModule, MatInputModule, MatDatepickerModule, 
    MatNativeDateModule, MatCardModule, MatButtonModule,
    MatCheckboxModule, MatSelectModule, MatChipsModule,
    MatRadioModule, 

  ],
  providers: [
    DatePipe,
    provideAnimations(),
    provideToastr()
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
