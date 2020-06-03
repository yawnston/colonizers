import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerPortraitComponent } from './player-portrait.component';

describe('PlayerPortraitComponent', () => {
  let component: PlayerPortraitComponent;
  let fixture: ComponentFixture<PlayerPortraitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerPortraitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayerPortraitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
