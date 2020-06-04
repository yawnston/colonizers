import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { PlayerInfo, GameState } from '../services/game/models/gamestate';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.css']
})
export class PlayerComponent implements OnInit {
  @Input() player: PlayerInfo;
  @Input() isLoading$: Observable<boolean>;
  @Input() gameState: GameState;
  @Output() onBuild = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {

  }

  getPlayerPoints(): number {
    let points = 0;
    for (const m of this.player.colony) {
      points += m.victoryValue;
    }
    return points;
  }

  build(index: number) {
    this.onBuild.next(index);
  }
}
