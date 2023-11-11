import { EventEmitter, Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export const LANGUAGE:string = "LANGUAGE"

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  
  private languageChanged = new EventEmitter<string>();

  constructor(private translate: TranslateService) {}

  setLanguage(language: string) {
    this.translate.use(language);
    localStorage.setItem(LANGUAGE, language);
    this.languageChanged.emit(language);
  }

  setLanguageFromLocaStoragel(){
    var localLanguage = localStorage.getItem(LANGUAGE);
    this.translate.use(localLanguage ?? "en")
    this.languageChanged.emit(localLanguage ?? "en");
  }

  getLanguage() {
    return this.translate.currentLang;
  }

  getTranslate(key: string){
    return this.translate.instant(key);
  }
  
  getLanguageChangedEvent() {
    return this.languageChanged;
  }

}
