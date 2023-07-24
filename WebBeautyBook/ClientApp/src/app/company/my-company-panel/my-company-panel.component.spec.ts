import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyCompanyPanelComponent } from './my-company-panel.component';

describe('MyCompanyPanelComponent', () => {
  let component: MyCompanyPanelComponent;
  let fixture: ComponentFixture<MyCompanyPanelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MyCompanyPanelComponent]
    });
    fixture = TestBed.createComponent(MyCompanyPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
