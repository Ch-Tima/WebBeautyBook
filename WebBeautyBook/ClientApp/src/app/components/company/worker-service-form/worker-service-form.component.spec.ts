import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkerServiceFormComponent } from './worker-service-form.component';

describe('WorkerServiceFormComponent', () => {
  let component: WorkerServiceFormComponent;
  let fixture: ComponentFixture<WorkerServiceFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WorkerServiceFormComponent]
    });
    fixture = TestBed.createComponent(WorkerServiceFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
