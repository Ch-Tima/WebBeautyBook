import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditScheduleTimeDialogComponent } from './EditScheduleTimeDialogComponent';

describe('EditScheduleTimeDialogComponent', () => {
  let component: EditScheduleTimeDialogComponent;
  let fixture: ComponentFixture<EditScheduleTimeDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditScheduleTimeDialogComponent]
    });
    fixture = TestBed.createComponent(EditScheduleTimeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
