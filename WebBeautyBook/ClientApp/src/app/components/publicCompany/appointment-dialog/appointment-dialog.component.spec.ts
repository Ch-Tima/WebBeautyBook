import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentDialogComponent } from './appointment-dialog.component';

describe('AppointmentDialogComponent', () => {
  let component: AppointmentDialogComponent;
  let fixture: ComponentFixture<AppointmentDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AppointmentDialogComponent]
    });
    fixture = TestBed.createComponent(AppointmentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
