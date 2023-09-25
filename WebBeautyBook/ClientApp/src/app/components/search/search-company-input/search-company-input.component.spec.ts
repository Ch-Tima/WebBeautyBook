import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchCompanyInputComponent } from './search-company-input.component';

describe('SearchCompanyInputComponent', () => {
  let component: SearchCompanyInputComponent;
  let fixture: ComponentFixture<SearchCompanyInputComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchCompanyInputComponent]
    });
    fixture = TestBed.createComponent(SearchCompanyInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
