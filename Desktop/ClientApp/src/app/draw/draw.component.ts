import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { GameState } from '../services/game/models/gamestate';

@Component({
  selector: 'app-draw',
  templateUrl: './draw.component.html',
  styleUrls: ['./draw.component.css']
})
export class DrawComponent implements OnInit {

  @Input() gameState: GameState;
  @Output() onPick = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
  }

  omnium() {
    this.onPick.next(this.gameState.actions.findIndex(x => x.type === 'TakeOmnium'));
  }

  draw() {
    if (this.canDraw()) {
      this.onPick.next(this.gameState.actions.findIndex(x => x.type === 'DrawModules'));
    }
  }

  canDraw(): boolean {
    return this.gameState.actions.find(x => x.type === 'DrawModules') !== undefined;
  }

}
