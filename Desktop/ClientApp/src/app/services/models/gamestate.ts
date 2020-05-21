export interface GameState {
  gameOver: boolean;
  gameEndInfo: GameEndInfo | undefined;
  boardState: BoardState;
  actions: GameAction[];
}

export interface GameEndInfo {
  players: PlayerEndInfo[];
}

export interface PlayerEndInfo {
  player: PlayerInfo;
  victoryPoints: number;
  ranking: number;
}

export interface PlayerInfo {
  colonist: Colonist;
  omnium: number;
  hand: Module[];
  colony: Module[];
  id: number;
}

export interface Colonist {
  name: string;
}

export interface Module {
  name: string;
  type: Color;
  buildCost: number;
  victoryValue: number;
}

export enum Color {
  green = 'Green',
  blue = 'Blue',
  red = 'Red',
  none = 'None'
}

export interface BoardState {
  players: PlayerInfo[];
  availableColonists: Colonist[];
  deck: Module[];
  discardTempStorage: Module[];
  playerTurn: number;
  gamePhase: string;
}

export interface GameAction {
  type: string;
  colonist: string | undefined;
  module: string | undefined;
  target: string | undefined;
}
