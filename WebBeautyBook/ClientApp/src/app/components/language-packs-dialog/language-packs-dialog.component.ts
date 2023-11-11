import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TranslationService } from 'src/app/services/translation/translation.service';

@Component({
  selector: 'app-language-packs-dialog',
  templateUrl: './language-packs-dialog.component.html',
  styleUrls: ['./language-packs-dialog.component.css']
})
export class LanguagePacksDialogComponent {

  constructor(public translation: TranslationService, private dialog: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public data: any){
    this.dialog.backdropClick().subscribe(s => this.dialog.close(this.translation.getLanguage()))
  }

  public flags = [{
    languageCode: 'en',
    flag: "us.svg",
    title: "United States"
  }, {
    languageCode: 'ua',
    flag: "ua.svg",
    title: "Ukraine"
  }, {
    languageCode: 'de',
    flag: "de.svg",
    title: "German"
  }, {
    languageCode: 'ru',
    flag: "ru.svg",
    title: "Russia"
  }, {
    languageCode: 'pl',
    flag: "pl.svg",
    title: "Poland"
  }];

  public getSelectedLanguage(language:string){
    this.dialog.close(language||'en');
  }

}
