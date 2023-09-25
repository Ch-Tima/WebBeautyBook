import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceFormComponent } from './service-form.component';

describe('ServiceFormComponent', () => {
  let component: ServiceFormComponent;
  let fixture: ComponentFixture<ServiceFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ServiceFormComponent]
    });
    fixture = TestBed.createComponent(ServiceFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
