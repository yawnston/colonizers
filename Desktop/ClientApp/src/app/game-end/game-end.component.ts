import { Component, OnInit, Input } from '@angular/core';
import { GameEndInfo } from '../services/models/gamestate';

@Component({
  selector: 'app-game-end',
  templateUrl: './game-end.component.html',
  styleUrls: ['./game-end.component.css']
})
export class GameEndComponent implements OnInit {

  @Input() gameEndInfo: GameEndInfo;

  constructor() { }

  ngOnInit() {
  }

}
