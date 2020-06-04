import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Module, PlayerInfo, GameState } from '../services/game/models/gamestate';

@Component({
  selector: 'app-player-hand',
  templateUrl: './player-hand.component.html',
  styleUrls: ['./player-hand.component.css']
})
export class PlayerHandComponent implements OnInit {

  @Input() hand: Module[];
  @Input() player: PlayerInfo;
  @Input() gameState: GameState;
  @Input() hideInformation: boolean;
  @Output() onPick = new EventEmitter<number>();

  get fullHand(): Module[] {
    let arr = [undefined, undefined, undefined, undefined, undefined];
    for (let i = 0; i < this.hand.length; i++) {
      arr[i] = this.hand[i];
    }
    return arr;
  }

  canBuild(index: number): boolean {
    return this.isCurrentPlayerBuildPhase() && this.fullHand[index] && this.player.omnium >= this.fullHand[index].buildCost;
  }

  build(index: number) {
    if (this.isCurrentPlayerBuildPhase()) {
      this.onPick.next(this.gameState.actions.findIndex(x => x.module === this.fullHand[index].name));
    }
  }

  isCurrentPlayerBuildPhase(): boolean {
    return this.player.id === this.gameState.boardState.playerTurn
      && this.gameState.boardState.gamePhase === 'Build';
  }

  constructor() { }

  ngOnInit() {
  }

}
