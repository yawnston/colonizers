import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { GameState, Module } from '../services/game/models/gamestate';

@Component({
  selector: 'app-discard',
  templateUrl: './discard.component.html',
  styleUrls: ['./discard.component.css']
})
export class DiscardComponent implements OnInit {

  @Input() gameState: GameState;
  @Output() onPick = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
  }

  getModules(): Module[] {
    // Find the appropriate modules in the temp discard storage
    return this.gameState.actions.map(x => this.gameState.boardState.discardTempStorage.find(y => y.name === x.module));
  }

  keep(module: Module) {
    this.onPick.next(this.gameState.actions.findIndex(x => x.module == module.name));
  }

}
