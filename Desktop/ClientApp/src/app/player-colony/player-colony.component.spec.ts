import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerColonyComponent } from './player-colony.component';

describe('PlayerColonyComponent', () => {
  let component: PlayerColonyComponent;
  let fixture: ComponentFixture<PlayerColonyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerColonyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayerColonyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
