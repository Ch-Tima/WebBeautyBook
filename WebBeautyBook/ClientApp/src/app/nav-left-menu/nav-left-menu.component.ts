import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, AfterViewInit, HostListener } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { NavMenuItem } from '../models/NavMenuItem';

@Component({
  selector: 'app-nav-left-menu',
  templateUrl: './nav-left-menu.component.html',
  styleUrls: ['./nav-left-menu.component.css']
})
export class NavLeftMenuComponent implements AfterViewInit, OnDestroy {

  
  @Input()
  public list: NavMenuItem[] = [];
  @Input()
  public headerText: string = "Menu";

  @Output() 
  resultEmitter = new EventEmitter<string>();

  private lastChangeMenu: any;
  private breakpointObserverMenu: any;

  constructor(private nav: NavbarService){
    nav.style = 'w75right';
  }

  ngOnDestroy(): void {
    this.nav.style = "normal";
  }

  ngAfterViewInit(): void {
    this.lastChangeMenu = document.getElementById('i:0');
    this.lastChangeMenu.classList.remove("li-hover");
    this.lastChangeMenu.classList.add("li-select");
    if(this.lastChangeMenu != null && this.lastChangeMenu != undefined){
      this.resultEmitter.emit(this.list[0].value);
    }
  }

  @HostListener('window:resize', ['$event'])
  private onResize(event:any) {
    var innerWidth = window.innerWidth;
    if (innerWidth > 768) {
      this.nav.style = 'w75right'
    }else{
      this.nav.style = 'normal'
    }
  }

  public onClick(event: any, value: string){
    var newItem = event.target;
    if(this.lastChangeMenu == newItem) return;
    
    newItem.classList.remove("li-hover");
    newItem.classList.add("li-select");

    this.lastChangeMenu.classList.remove("li-select");
    this.lastChangeMenu.classList.add("li-hover");

    this.lastChangeMenu = newItem;

    this.resultEmitter.emit(value);
  }

}