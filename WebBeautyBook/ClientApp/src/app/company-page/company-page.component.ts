import { Component } from '@angular/core';
import { Service } from '../models/Service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { BaseUser } from '../models/BaseUser';

@Component({
  selector: 'app-company-page',
  templateUrl: './company-page.component.html',
  styleUrls: ['./company-page.component.css']
})
export class CompanyPageComponent {

  public services:Service[] = []
  public comments:any[] = []

  constructor(private http: HttpClient, private auth: AuthService){
    for(var i = 0; i < 9; i++){
      this.services.push(new Service())
      if(i % 2 == 0){
        this.comments.push(i);
      }
    }
    
  }


}
