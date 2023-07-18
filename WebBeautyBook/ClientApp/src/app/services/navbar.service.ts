import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {

  public style: "normal"|"w75right" = "normal";

  constructor() { }

}
