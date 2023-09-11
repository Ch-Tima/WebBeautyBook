import { TestBed } from '@angular/core/testing';

import { FooterDisplayService } from './footer-display.service';

describe('FooterDisplayService', () => {
  let service: FooterDisplayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FooterDisplayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
