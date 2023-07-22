import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitationEmployeeComponent } from './invitation-employee.component';

describe('InvitationEmployeeComponent', () => {
  let component: InvitationEmployeeComponent;
  let fixture: ComponentFixture<InvitationEmployeeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InvitationEmployeeComponent]
    });
    fixture = TestBed.createComponent(InvitationEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
