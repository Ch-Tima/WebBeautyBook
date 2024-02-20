import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyScheduleComponent } from './company-schedule.component';

describe('CompanyScheduleComponent', () => {
  let component: CompanyScheduleComponent;
  let fixture: ComponentFixture<CompanyScheduleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyScheduleComponent]
    });
    fixture = TestBed.createComponent(CompanyScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
