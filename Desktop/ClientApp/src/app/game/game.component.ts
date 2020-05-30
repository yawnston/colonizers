import { Component, OnInit, OnDestroy } from '@angular/core';
import { GameState } from '../services/models/gamestate';
import { GameService } from '../services/game.service';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css'],
  host: { class: 'vertical-flex' },
})
export class GameComponent implements OnInit, OnDestroy {
  gameState: GameState;
  playerLoadingObs: Observable<boolean>[] = [...Array(4)].map(_ => of(false));

  constructor(private gameService: GameService) {

  }

  processTurn(): void {
    if (false) {
      // TODO: human player
      this.processHumanTurn();
    }
    else {
      // AI player
      this.processAITurn();
    }
  }

  processAITurn(): void {
    if (this.gameState && !this.gameState.gameOver) {
      const playerTurn = this.gameState.boardState.playerTurn;
      this.playerLoadingObs[playerTurn - 1] = this.gameService.isLoading$;

      this.gameService.processAITurn$().subscribe(x => {
        this.playerLoadingObs[playerTurn - 1] = of(false);
        this.gameState = x;
        this.processTurn();
      });
    }
  }

  processHumanTurn(): void {
    // TODO: process human turn (somehow set turn to enable actions and end)
  }

  ngOnInit(): void {
    this.gameService.initGame$().subscribe(x => {
      this.gameState = x;
    });
  }

  ngOnDestroy(): void {
    this.gameService.disposeGame();
  }
}
