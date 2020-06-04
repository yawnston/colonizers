import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-player-portrait',
  templateUrl: './player-portrait.component.html',
  styleUrls: ['./player-portrait.component.css']
})
export class PlayerPortraitComponent implements OnInit {

  @Input() colonist: string;
  @Input() hideInformation: boolean;

  constructor() { }

  ngOnInit() {
  }

}
