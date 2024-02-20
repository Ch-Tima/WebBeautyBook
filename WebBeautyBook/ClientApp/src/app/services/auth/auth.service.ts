import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {LoginModel} from "../../models/LoginModel";
import {JWT} from "../../models/JWT ";
import {RegisterModel} from "../../models/RegisterModel";
import {ResetPasswordModel} from "../../models/ResetPasswordModel";
import {UserDataModel} from "../../models/UserDataModel";

export const TOKEN: string = "token"
export const EXPRARTION_TOKEN: string = "expiration-token"
export const USER_DATA: string = "user-data"

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) {
  }

  /**API request login*/
  public login(data: LoginModel): Observable<JWT> {
    return this.http.post<JWT>("api/Auth/login", data);
  }

  /**API request login*/
  public refreshTokens(): Observable<JWT> {
    return this.http.post<JWT>("api/Auth/refreshTokens", {}, {
      headers: this.getHeadersWithToken()
    });
  }

  /**API request register*/
  public register(data: RegisterModel): Observable<any> {
    return this.http.post<any>("api/Auth/register", data);
  }

  /**API request forgotPassword*/
  public ForgotPassword(emailJsonString:string): Observable<string> {
    return this.http.post<any>("api/Auth/forgotPassword", JSON.stringify(emailJsonString), {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    });
  }

  /**API request resetPassword*/
  public ResetPassword(data: ResetPasswordModel): Observable<any>{
    return this.http.post<any>("api/Auth/resetPassword", data);
  }

  /**API request emailConfirmation*/
  public emailConfirmation(token: string, email: string): Observable<any>{
    return this.http.get("api/Auth/confirmEmail", {
      params : {
        token: token,
        email: email
      }
    });
  }

  /**API request registerOwnCompany (OWN_COMPANY)*/
  public registerOwnCompany(data: RegisterModel): Observable<UserDataModel> | null {
    var userData = this.getLocalUserDate();
    if(userData != null && userData.roles.filter(role => role == 'admin').length == 0)
      return null;
    return this.http.post<any>("api/Auth/registrationViaAdmin", data, {
      headers: new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem(TOKEN))
    });
  }

  /**API request registerOwnCompany (OWN_COMPANY)*/
  public registerWorker(data: RegisterModel): Observable<UserDataModel> | null {
    var userData = this.getLocalUserDate();
    if(userData != null && userData.roles.filter(role => role == 'own_company').length == 0)
      return null;
    return this.http.post<any>("api/Auth/registrationViaCompany", data, {
      headers: new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem(TOKEN))
    });
  }

  /**API request getUserData*/
  public getUserData(): Observable<UserDataModel> {
    return this.http.get<UserDataModel>("api/User", {
      headers: new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem(TOKEN))
    });
  }
  /**
   * Reads user data from "localStorage"
   * @returns UserDataModel or null
  */
  public getLocalUserDate() : UserDataModel|null{
    var json_data = localStorage.getItem(USER_DATA)
    if(json_data != null){
      return new BehaviorSubject<UserDataModel>(JSON.parse(json_data)).value;
    }else{
      return null;
    }
  }

  /**
   * Remove all dates from "localStorage".
   */
  public signOut() {
    localStorage.clear();
    window.location.replace("/");
  }

  /**
   * Checks for the presence of a token.
   * @returns true if the token exists, or false if the token does not exist.
   */
  public hasToken(): boolean {
    const token = localStorage.getItem(TOKEN);
    return token != null;
  }

  /**
   * Store the token and the expired in "localStorage".
   * @param token
   * @param expration
   */
  public saveToken(token: string, expration: string) {
    localStorage.setItem(TOKEN, token);
    localStorage.setItem(EXPRARTION_TOKEN, expration);
  }

  /**
   * Stores user data in "localStorage"
   * @param user - user data
   */
  public saveUserData(user: UserDataModel){
    localStorage.setItem(USER_DATA, JSON.stringify(user));
  }

  /**
   * Creates {@link HttpHeaders} with a custom token
   * @returns 'HttpHeaders' with Authorization Token
   */
  public getHeadersWithToken(): HttpHeaders{
    return new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem(TOKEN))
  }

  public getUserFromLocalStorage(): UserDataModel | null{
    const userDataJson = localStorage.getItem(USER_DATA);
    if (userDataJson) {
      const userDataObj = JSON.parse(userDataJson);
      return this.mapToUserDataModel(userDataObj);
    }
    return null;
  }

  private mapToUserDataModel(userDataObj: any): UserDataModel {
    const userData = new UserDataModel();
    userData.name = userDataObj.name || '';
    userData.surname = userDataObj.surname || '';
    userData.email = userDataObj.email || '';
    userData.photo = userDataObj.photo || '';
    userData.phoneNumber = userDataObj.phoneNumber || '';
    userData.roles = userDataObj.roles || [];
    userData.companyId = userDataObj.companyId || undefined;
    userData.workerId = userDataObj.workerId || undefined;
    return userData;
  }

}
