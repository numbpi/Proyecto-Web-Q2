import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMediators } from './admin-mediators';

describe('AdminMediators', () => {
  let component: AdminMediators;
  let fixture: ComponentFixture<AdminMediators>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminMediators],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminMediators);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
