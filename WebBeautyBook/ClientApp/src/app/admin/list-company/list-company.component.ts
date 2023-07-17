import { Component, Input } from '@angular/core';
import { Company } from 'src/app/models/Company';

@Component({
  selector: 'app-list-company',
  templateUrl: './list-company.component.html',
  styleUrls: ['./list-company.component.css']
})
export class ListCompanyComponent {

  @Input() listCompanies: Company[] = [];

}
