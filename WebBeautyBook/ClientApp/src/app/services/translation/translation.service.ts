import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export const LANGUAGE:string = "LANGUAGE"

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  
  constructor(private translate: TranslateService) {}

  setLanguage(language: string) {
    this.translate.use(language);
    localStorage.setItem(LANGUAGE, language);
  }

  setLanguageFromLocaStoragel(){
    var localLanguage = localStorage.getItem(LANGUAGE);
    this.translate.use(localLanguage ?? "en")
  }

  getLanguage() {
    return this.translate.currentLang;
  }

  getTranslate(key: string){
    return this.translate.instant(key);
  }



}
