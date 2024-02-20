import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyContactsComponent } from './company-contacts.component';

describe('CompanyContactsComponent', () => {
  let component: CompanyContactsComponent;
  let fixture: ComponentFixture<CompanyContactsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyContactsComponent]
    });
    fixture = TestBed.createComponent(CompanyContactsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
