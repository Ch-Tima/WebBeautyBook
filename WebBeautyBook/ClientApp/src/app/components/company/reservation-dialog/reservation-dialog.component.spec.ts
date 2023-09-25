import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationDialogComponent } from './reservation-dialog.component';

describe('ReservationDialogComponent', () => {
  let component: ReservationDialogComponent;
  let fixture: ComponentFixture<ReservationDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReservationDialogComponent]
    });
    fixture = TestBed.createComponent(ReservationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
