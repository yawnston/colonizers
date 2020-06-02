import { Component, OnInit } from '@angular/core';
import { ScriptsService } from '../services/scripts/scripts.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  host: {class: 'vertical-flex'},
})
export class HomeComponent implements OnInit {

  scripts: string[] = [];
  selectedPlayers: string[] = [undefined, undefined, undefined, undefined];

  constructor(private scriptsService: ScriptsService) { }

  ngOnInit() {
    this.scriptsService.getScripts$().subscribe((result) => {
      this.scripts = result;
    });
  }

  onPlayerSelect(playerName: string, playerNumber: number) {
    this.selectedPlayers[playerNumber - 1] = playerName;
  }

}
