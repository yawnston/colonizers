import { Component, OnInit, OnDestroy } from '@angular/core';
import { GameState } from '../services/game/models/gamestate';
import { GameService } from '../services/game/game.service';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css'],
  host: { class: 'vertical-flex' },
})
export class GameComponent implements OnInit, OnDestroy {
  playerNames: string[];
  gameState: GameState;

  // Observables indicating whether the given AI player is currently calculating a decision.
  playerLoadingObs: Observable<boolean>[] = [...Array(4)].map(_ => of(false));

  // Indicates whether the game has started.
  isGameRunning: boolean = false;

  // Indicates whether the game is waiting on input from a human player
  isWaitingForHumanPlayer: boolean = false;

  constructor(private gameService: GameService,
    private router: Router) { }

  processTurn(): void {
    this.isGameRunning = true;
    if (this.isHumanTurn()) {
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
    this.isWaitingForHumanPlayer = true;
  }

  onHumanPlayerAction(actionNumber: number) {
    this.gameService.processPlayerTurn$(actionNumber).subscribe((result) => {
      this.gameState = result;
      this.isWaitingForHumanPlayer = false;
      this.processTurn();
    });
  }

  abandonGame(): void {
    this.router.navigateByUrl('/');
  }

  isHumanTurn(): boolean {
    return this.playerNames[this.gameState.boardState.playerTurn - 1] === 'Human Player';
  }

  isColonistPickPhase(): boolean {
    return this.gameState.boardState.gamePhase === 'ColonistPick';
  }

  isDrawPhase(): boolean {
    return this.gameState.boardState.gamePhase === 'Draw';
  }

  isDiscardPhase(): boolean {
    return this.gameState.boardState.gamePhase === 'Discard';
  }

  isPowerPhase(): boolean {
    return this.gameState.boardState.gamePhase === 'Power';
  }

  isBuildPhase(): boolean {
    return this.gameState.boardState.gamePhase === 'Build';
  }

  ngOnInit(): void {
    this.gameState = this.gameService.initialGameState;
    this.playerNames = this.gameService.playerNames;
  }

  ngOnDestroy(): void {
    this.gameService.disposeGame();
  }
}
