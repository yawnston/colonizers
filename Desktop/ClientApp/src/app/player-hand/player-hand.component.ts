import { Component, OnInit, Input } from '@angular/core';
import { Module } from '../services/game/models/gamestate';

@Component({
  selector: 'app-player-hand',
  templateUrl: './player-hand.component.html',
  styleUrls: ['./player-hand.component.css']
})
export class PlayerHandComponent implements OnInit {

  @Input() hand: Module[];

  get fullHand(): Module[] {
    let arr = [undefined, undefined, undefined, undefined, undefined];
    for (let i = 0; i < this.hand.length; i++) {
      arr[i] = this.hand[i];
    }
    return arr;
  }

  constructor() { }

  ngOnInit() {
  }

}
