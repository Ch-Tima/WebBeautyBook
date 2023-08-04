import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyPageComponent } from './company-page.component';

describe('CompanyPageComponent', () => {
  let component: CompanyPageComponent;
  let fixture: ComponentFixture<CompanyPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyPageComponent]
    });
    fixture = TestBed.createComponent(CompanyPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
