import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeesPanelComponent } from './employees-panel.component';

describe('EmployeesPanelComponent', () => {
  let component: EmployeesPanelComponent;
  let fixture: ComponentFixture<EmployeesPanelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EmployeesPanelComponent]
    });
    fixture = TestBed.createComponent(EmployeesPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
