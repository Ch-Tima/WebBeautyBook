import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavLeftMenuComponent } from './nav-left-menu.component';

describe('NavLeftMenuComponent', () => {
  let component: NavLeftMenuComponent;
  let fixture: ComponentFixture<NavLeftMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NavLeftMenuComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavLeftMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
