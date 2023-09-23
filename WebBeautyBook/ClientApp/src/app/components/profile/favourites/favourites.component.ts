import { HttpClient } from '@angular/common/http';
import {Component, OnInit} from '@angular/core';
import { Company } from 'src/app/models/Company';
import { CompanyLike } from 'src/app/models/CompanyLike';
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-favourites',
  templateUrl: './favourites.component.html',
  styleUrls: ['./favourites.component.css']
})
export class FavouritesComponent implements OnInit{

  public likes: CompanyLike[]|undefined;

  constructor(private auth: AuthService, private http: HttpClient){
  }

  public async ngOnInit() {
    await this.getAllMienLikes()
  }

  public async remove(item: Company) {
    // Find the index of the company to remove from the liked list
    const i = this.likes?.findIndex(x => x.companyId == item.id);
    if(i != undefined && i != -1){
      this.likes?.splice(i, 1);
    }else{//reload
      this.likes = undefined;
      await this.getAllMienLikes();
    }
  }

  private async getAllMienLikes(){
    // Retrieve liked companies from the
    try {
      // Retrieve liked companies from the API using an async/await pattern
      const res = await this.http.get<CompanyLike[]>("api/CompanyLike", {
        headers: this.auth.getHeadersWithToken()
      }).toPromise();
      // Mark each liked company as a favorite
      res?.forEach((x) => {
        if (x != undefined && x.company != undefined) {
          x.company.isFavorite = true;
        }
      });
      this.likes = res;// Set the liked companies in the component
    } catch (er) {
      console.log(er);
    }
  }

}
