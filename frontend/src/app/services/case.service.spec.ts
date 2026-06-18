import { TestBed } from '@angular/core/testing';

import { Case } from './case.service';

describe('Case', () => {
  let service: Case;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Case);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
