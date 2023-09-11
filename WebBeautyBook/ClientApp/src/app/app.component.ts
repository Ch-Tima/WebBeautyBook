import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { FooterDisplayService } from './services/footer/footer-display.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  
  title = 'app';
  showFooter: boolean = true;

  constructor(private router: Router, private footerDisplayService: FooterDisplayService) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.showFooter = this.footerDisplayService.shouldDisplayFooter();
      }
    });
  }
}
