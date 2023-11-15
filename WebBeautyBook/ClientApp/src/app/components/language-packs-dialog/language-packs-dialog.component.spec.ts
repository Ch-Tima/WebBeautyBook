import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LanguagePacksDialogComponent } from './language-packs-dialog.component';

describe('LanguagePacksDialogComponent', () => {
  let component: LanguagePacksDialogComponent;
  let fixture: ComponentFixture<LanguagePacksDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LanguagePacksDialogComponent]
    });
    fixture = TestBed.createComponent(LanguagePacksDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
