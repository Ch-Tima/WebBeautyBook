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
    language = language || "en";
    this.translate.use(language);
    localStorage.setItem(LANGUAGE, language);
    this.languageChanged.emit(language);
  }

  setLanguageFromLocaStoragel(){
    var localLanguage = localStorage.getItem(LANGUAGE) || "en"
    this.translate.use(localLanguage)
    this.languageChanged.emit(localLanguage)
  }

  getLanguage() {
    return this.translate.currentLang || "en";
  }

  getTranslate(key: string){
    return this.translate.instant(key);
  }

  getTranslationByKey(key: string) {
    return this.translate.get(key);
  }
  
  getLanguageChangedEvent() {
    return this.languageChanged;
  }

}
