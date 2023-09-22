import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Company } from 'src/app/models/Company';
import { CompanyLike } from 'src/app/models/CompanyLike';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-favourites',
  templateUrl: './favourites.component.html',
  styleUrls: ['./favourites.component.css']
})
export class FavouritesComponent {

  public likes: CompanyLike[]|undefined;

  constructor(private auth: AuthService, private http: HttpClient){
    this.getAllMienLikes()
  }

  public remove(item: Company) {

    var i = this.likes?.findIndex(x => x.companyId == item.id);
    if(i != undefined && i != -1){
      this.likes?.splice(i, 1);
    }else{//reload
      this.likes = undefined;
      this.getAllMienLikes();
    }
  }

  private getAllMienLikes(){
    this.http.get<CompanyLike[]>("api/CompanyLike", {
      headers: this.auth.getHeadersWithToken()
    }).subscribe(
      res => {
        res.forEach((x) => {
          if(x != undefined && x.company != undefined){
            x.company.isFavorite = true;
          }
        });
        this.likes = res;
      }, er => {
        console.log(er);
      }
    )
  }

}
