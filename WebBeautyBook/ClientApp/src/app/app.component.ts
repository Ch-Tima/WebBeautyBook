import {Component, OnInit} from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { FooterDisplayService } from './services/footer/footer-display.service';
import {LocationService} from "./services/location/location.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit{

  title = 'app';
  showFooter: boolean = true;

  constructor(private router: Router, private footerDisplayService: FooterDisplayService, private location: LocationService) {
  }
  
  public async ngOnInit() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.showFooter = this.footerDisplayService.shouldDisplayFooter();
      }
    });
  }
}
