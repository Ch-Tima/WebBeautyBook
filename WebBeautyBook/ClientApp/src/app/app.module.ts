import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { BrowserAnimationsModule, provideAnimations } from "@angular/platform-browser/animations";
import { FullCalendarModule } from '@fullcalendar/angular';
import { DragScrollModule } from 'ngx-drag-scroll';
import { NgxMatTimepickerModule } from "ngx-mat-timepicker"
import { ToastrModule, provideToastr } from 'ngx-toastr';
import { MatButtonModule } from '@angular/material/button';
import { FilterPipe } from './filterPipes/FilterPipe';
import { DatePipe } from '@angular/common';
import { LocationService } from "./services/location/location.service";
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
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
//@ngx-translate
import { TranslateModule, TranslateLoader, TranslateModuleConfig } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

//Components
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { FooterComponent } from './components/footer/footer.component';
import { PrivacyPolicyComponent } from './components/privacy-policy/privacy-policy.component';
//Nav_Menu
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { NavLeftMenuComponent } from './components/nav-left-menu/nav-left-menu.component';
//Auth
import { RegisterPageComponent } from './components/auth/register-page/register-page.component';
import { LoginPageComponent } from './components/auth/login-page/login-page.component';
import { EmailConfirmationComponent } from './components/auth/email-confirmation/email-confirmation.component';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
//Admin
import { AdminPageComponent } from './components/admin/admin-page/admin-page.component';
import { CategotyPanelComponent } from './components/admin/categoty-panel/categoty-panel.component';
import { CategoryFormComponent } from './components/admin/category-form/category-form.component';
import { OwnCompanyPanelComponent } from './components/admin/own-company-panel/own-company-panel.component';
import { CompanyPanelComponent } from './components/admin/company-panel/company-panel.component';
import { ListCompanyComponent } from './components/admin/list-company/list-company.component';
import { CompanyFormComponent } from './components/admin/company-form/company-form.component';
import { LocationPanelComponent } from './components/admin/location-panel/location-panel.component';
import { LocationFormComponent } from './components/admin/location-form/location-form.component';
//Company
import { MyCompanyPanelComponent } from './components/company/my-company-panel/my-company-panel.component';
import { AppointmentComponent } from './components/company/appointment/appointment.component';
import { EmployeesPanelComponent } from './components/company/employees-panel/employees-panel.component';
import { ServicePanelComponent } from './components/company/service-panel/service-panel.component';
import { ServiceFormComponent } from './components/company/service-form/service-form.component';
import { WorkerServiceFormComponent } from './components/company/worker-service-form/worker-service-form.component';
import { InvitationEmployeeComponent } from './components/company/invitation-employee/invitation-employee.component';
import { AcceptInvitationPageComponent } from './components/company/accept-invitation-page/accept-invitation-page.component';
import { CompanyCardComponent } from './components/company/company-card/company-card.component';
import { ReservationDialogComponent } from './components/company/reservation-dialog/reservation-dialog.component';
//PublicCompany
import { CompanyPageComponent } from './components/publicCompany/company-page/company-page.component';
import { AppointmentDialogComponent } from './components/publicCompany/appointment-dialog/appointment-dialog.component';
//Profile
import { AppointmentListComponent } from './components/profile/appointment-list/appointment-list.component';
import { ProfilePageComponent } from './components/profile/profile-page/profile-page.component';
import { AccountSettingsComponent } from './components/profile/account-settings/account-settings.component';
import { FavouritesComponent } from './components/profile/favourites/favourites.component';
//Search
import { SearchPageComponent } from './components/search/search-page/search-page.component';
import { SearchCompanyInputComponent } from './components/search/search-company-input/search-company-input.component';
//ImageSliderComponent
import { ImageSliderComponent } from './components/image-slider/image-slider.component';
//LanguagePacksDialogComponent
import { LanguagePacksDialogComponent } from './components/language-packs-dialog/language-packs-dialog.component';

const routes: Routes = [
  //Public
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'search', component: SearchPageComponent },
  { path: 'privacyPolicy', component: PrivacyPolicyComponent },
   //AdminPanel
  { path: "adminPanel", component: AdminPageComponent },
  //CompanyPanel
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
    AppointmentListComponent,
    SearchPageComponent,
    SearchCompanyInputComponent,
    PrivacyPolicyComponent,
    LanguagePacksDialogComponent,
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
    MatRadioModule, MatTableModule, MatPaginatorModule,
    MatSortModule, MatProgressSpinnerModule, MatIconModule,
    //ngx-translate
    TranslateModule.forRoot({
      loader:{
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),

  ],
  providers: [
    DatePipe,
    LocationService,
    provideAnimations(),
    provideToastr()
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

export const translateConfig: TranslateModuleConfig = {
  defaultLanguage: 'en',
  useDefaultLang: true,
};