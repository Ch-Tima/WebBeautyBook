import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TranslationService } from '../translation/translation.service';

@Injectable({
  providedIn: 'root'
})
export class LanguageInterceptorService implements HttpInterceptor {

  constructor(private translation: TranslationService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let lang = this.translation.getLanguage();
    const clonedRequest = req.clone({ headers: req.headers.set('Accept-Language', lang) });
    return next.handle(clonedRequest);
  }
}
