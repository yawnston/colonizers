import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { GameState } from '../services/game/models/gamestate';

@Component({
  selector: 'app-power',
  templateUrl: './power.component.html',
  styleUrls: ['./power.component.css']
})
export class PowerComponent implements OnInit {

  @Input() gameState: GameState;
  @Output() onPick = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {

  }

  getAllColonistNames() {
    let colonistNames: string[] = [undefined, undefined, undefined, undefined, undefined];
    for (let i = 0; i < this.gameState.actions.length; i++) {
      colonistNames[i] = this.gameState.actions[i].target;
    }
    return colonistNames.filter((val) => val !== undefined);
  }

  hasChoice(): boolean {
    return this.gameState.actions.length > 1;
  }

  target(colonist) {
    if (colonist === undefined) {
      this.onPick.next(this.gameState.actions.findIndex(x => x.type === 'DoNothing'));
      return;
    }

    this.onPick.next(this.gameState.actions.findIndex(x => x.target === colonist));
  }
}
