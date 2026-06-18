import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCases } from './admin-cases';

describe('AdminCases', () => {
  let component: AdminCases;
  let fixture: ComponentFixture<AdminCases>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCases],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminCases);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
