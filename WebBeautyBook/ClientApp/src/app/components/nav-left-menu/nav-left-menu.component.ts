import { Component, EventEmitter, Input, OnDestroy, Output, AfterViewInit, HostListener } from '@angular/core';
import { NavMenuItem } from '../../models/NavMenuItem';
import {NavbarService} from "../../services/navbar/navbar.service";

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
    if(!this.lastChangeMenu){
      this.resultEmitter.emit(this.list[0].value);
    }
  }

  @HostListener('window:resize', ['$event'])
  private onResize(event:any) {
    const innerWidth = window.innerWidth;
    this.nav.style = innerWidth > 768 ? 'w75right' : 'normal';
  }

  public onClick(event: any, value: string){
    const newItem = event.target;
    if(this.lastChangeMenu == newItem) return;

    newItem.classList.remove("li-hover");
    newItem.classList.add("li-select");

    this.lastChangeMenu.classList.remove("li-select");
    this.lastChangeMenu.classList.add("li-hover");

    this.lastChangeMenu = newItem;

    this.resultEmitter.emit(value);
  }

}
