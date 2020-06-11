import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InterestsDetailComponent } from './interests-detail.component';

describe('InterestsDetailComponent', () => {
  let component: InterestsDetailComponent;
  let fixture: ComponentFixture<InterestsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InterestsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InterestsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
