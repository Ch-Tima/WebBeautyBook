import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyCardComponent } from './company-card.component';

describe('CompanyCardComponent', () => {
  let component: CompanyCardComponent;
  let fixture: ComponentFixture<CompanyCardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyCardComponent]
    });
    fixture = TestBed.createComponent(CompanyCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
