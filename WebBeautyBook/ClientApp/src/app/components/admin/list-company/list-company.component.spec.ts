import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListCompanyComponent } from './list-company.component';

describe('ListCompanyComponent', () => {
  let component: ListCompanyComponent;
  let fixture: ComponentFixture<ListCompanyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ListCompanyComponent]
    });
    fixture = TestBed.createComponent(ListCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
