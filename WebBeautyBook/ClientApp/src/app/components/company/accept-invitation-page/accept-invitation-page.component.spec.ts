import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcceptInvitationPageComponent } from './accept-invitation-page.component';

describe('AcceptInvitationPageComponent', () => {
  let component: AcceptInvitationPageComponent;
  let fixture: ComponentFixture<AcceptInvitationPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AcceptInvitationPageComponent]
    });
    fixture = TestBed.createComponent(AcceptInvitationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
