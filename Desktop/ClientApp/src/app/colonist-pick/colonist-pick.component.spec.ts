import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColonistPickComponent } from './colonist-pick.component';

describe('ColonistPickComponent', () => {
  let component: ColonistPickComponent;
  let fixture: ComponentFixture<ColonistPickComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColonistPickComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColonistPickComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
