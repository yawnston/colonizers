import { Component, OnInit, Input } from '@angular/core';
import { Module } from '../services/game/models/gamestate';

@Component({
  selector: 'app-player-colony',
  templateUrl: './player-colony.component.html',
  styleUrls: ['./player-colony.component.css']
})
export class PlayerColonyComponent implements OnInit {

  @Input() colony: Module[];

  get fullColony(): Module[] {
    let arr = [undefined, undefined, undefined, undefined, undefined];
    for (let i = 0; i < this.colony.length; i++) {
      arr[i] = this.colony[i];
    }
    return arr;
  }

  constructor() { }

  ngOnInit() {
  }

}
