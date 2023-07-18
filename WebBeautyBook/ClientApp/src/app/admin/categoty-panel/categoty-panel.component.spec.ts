import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategotyPanelComponent } from './categoty-panel.component';

describe('CategotyPanelComponent', () => {
  let component: CategotyPanelComponent;
  let fixture: ComponentFixture<CategotyPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategotyPanelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategotyPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
