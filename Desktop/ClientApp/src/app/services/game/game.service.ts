import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GameState } from './models/gamestate';
import { take, tap } from 'rxjs/operators';
import { Observable, BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  // Field used for storing the initial game state, since HomeComponent is the one doing the initialization
  initialGameState: GameState;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  public isLoading$: Subject<boolean> = new BehaviorSubject(false);

  public initGame$(playerNames: string[]): Observable<GameState> {
    this.isLoading$.next(true);
    return this.http.post<GameState>(this.baseUrl + 'api/game/start', playerNames)
      .pipe(
        take(1),
        tap(x => {
          this.isLoading$.next(false);
          this.initialGameState = x;
        }),
      );
  }

  public processAITurn$(): Observable<GameState> {
    this.isLoading$.next(true);
    return this.http.post<GameState>(this.baseUrl + 'api/game/aiturn', undefined)
      .pipe(
        take(1),
        tap(_ => this.isLoading$.next(false)),
      );
  }

  public processPlayerTurn$(move: number): Observable<GameState> {
    this.isLoading$.next(true);
    return this.http.post<GameState>(this.baseUrl + 'api/game/playerturn', move)
      .pipe(
        take(1),
        tap(_ => this.isLoading$.next(false)),
      );
  }

  public disposeGame(): void {
    this.isLoading$.next(true);
    this.http.post(this.baseUrl + 'api/game/dispose', undefined)
      .pipe(
        take(1),
      ).subscribe(_ => { this.isLoading$.next(false) });
  }
}
