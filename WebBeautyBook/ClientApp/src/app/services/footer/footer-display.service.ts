import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class FooterDisplayService {
  private hideFooterRoutes = ['profile', 'adminPanel', "companyPanel", "register", "forgotPassword", "resetPassword", "emailConfirmation"]; // Список маршрутов, на которых не нужен footer
  
  constructor(private router: Router) {}
  
  public shouldDisplayFooter(): boolean {
    const currentRoute = this.router.url;
    return !this.hideFooterRoutes.some(route => currentRoute.includes(route));
  }
}
