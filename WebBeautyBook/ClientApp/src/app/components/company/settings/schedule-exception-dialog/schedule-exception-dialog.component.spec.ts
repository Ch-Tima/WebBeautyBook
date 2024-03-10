import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleExceptionDialogComponent } from './schedule-exception-dialog.component';

describe('ScheduleExceptionDialogComponent', () => {
  let component: ScheduleExceptionDialogComponent;
  let fixture: ComponentFixture<ScheduleExceptionDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScheduleExceptionDialogComponent]
    });
    fixture = TestBed.createComponent(ScheduleExceptionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
