import { Component, OnInit, Input } from '@angular/core';
import { BoardState } from '../services/models/gamestate';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  @Input() boardState: BoardState;

  constructor() { }

  ngOnInit() {
  }

}
