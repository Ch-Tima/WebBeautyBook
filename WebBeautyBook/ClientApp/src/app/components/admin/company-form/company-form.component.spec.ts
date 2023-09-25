import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyFormComponent } from './company-form.component';

describe('CompanyFormComponent', () => {
  let component: CompanyFormComponent;
  let fixture: ComponentFixture<CompanyFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyFormComponent]
    });
    fixture = TestBed.createComponent(CompanyFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
