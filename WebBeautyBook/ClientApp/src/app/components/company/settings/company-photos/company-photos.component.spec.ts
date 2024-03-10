import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyPhotosComponent } from './company-photos.component';

describe('CompanyPhotosComponent', () => {
  let component: CompanyPhotosComponent;
  let fixture: ComponentFixture<CompanyPhotosComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyPhotosComponent]
    });
    fixture = TestBed.createComponent(CompanyPhotosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
