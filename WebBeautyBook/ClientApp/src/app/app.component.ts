import {Component, OnInit} from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { FooterDisplayService } from './services/footer/footer-display.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit{

  title = 'app';
  // Initialize showFooter to true by default
  showFooter: boolean = true;

  constructor(private router: Router, private footerDisplayService: FooterDisplayService) {}


  // ngOnInit is called when the component is initialized
  public async ngOnInit() {
    // Subscribe to router events to track route changes
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // When a navigation event (route change) occurs, update showFooter based on service logic
        this.showFooter = this.footerDisplayService.shouldDisplayFooter();
      }
    });
  }
}
