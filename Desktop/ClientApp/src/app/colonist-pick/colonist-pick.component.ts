import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { GameState } from '../services/game/models/gamestate';

@Component({
  selector: 'app-colonist-pick',
  templateUrl: './colonist-pick.component.html',
  styleUrls: ['./colonist-pick.component.css']
})
export class ColonistPickComponent implements OnInit {

  @Input() gameState: GameState;
  @Output() onPick = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
    
  }

  getAllColonistNames() {
    let colonistNames = [undefined, undefined, undefined, undefined, undefined, undefined];
    for (let i = 0; i < this.gameState.actions.length; i++) {
      colonistNames[i] = this.gameState.actions[i].colonist;
    }
    return colonistNames;
  }

  pick(colonist) {
    if (colonist) {
      const index = this.gameState.actions.findIndex(x => x.colonist == colonist);
      this.onPick.next(index);
    }
  }

}
