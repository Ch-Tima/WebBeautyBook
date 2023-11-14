import { TestBed } from '@angular/core/testing';

import { LanguageInterceptorService } from './language-interceptor.service';

describe('LanguageInterceptorService', () => {
  let service: LanguageInterceptorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LanguageInterceptorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
