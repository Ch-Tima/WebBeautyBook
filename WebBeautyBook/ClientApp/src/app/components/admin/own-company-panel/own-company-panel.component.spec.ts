import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OwnCompanyPanelComponent } from './own-company-panel.component';

describe('OwnCompanyPanelComponent', () => {
  let component: OwnCompanyPanelComponent;
  let fixture: ComponentFixture<OwnCompanyPanelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OwnCompanyPanelComponent]
    });
    fixture = TestBed.createComponent(OwnCompanyPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
