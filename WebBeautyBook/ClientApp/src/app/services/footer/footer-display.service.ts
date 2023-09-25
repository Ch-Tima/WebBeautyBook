import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class FooterDisplayService {
  //List of routes that don't need a footer
  private hideFooterRoutes = ['profile', 'adminPanel', "companyPanel", "register", "forgotPassword", "resetPassword", "emailConfirmation"]; //
  constructor(private router: Router) {}

  // Function to determine if the footer should be displayed on the current route
  public shouldDisplayFooter(): boolean {
    // Get the current route URL
    const currentRoute = this.router.url;
    // Check if the current route URL contains any of the routes in hideFooterRoutes
    // If it does, return false (don't display the footer), otherwise return true
    return !this.hideFooterRoutes.some(route => currentRoute.includes(route));
  }
}
