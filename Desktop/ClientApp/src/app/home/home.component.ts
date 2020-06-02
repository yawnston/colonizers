import { Component, OnInit } from '@angular/core';
import { ScriptsService } from '../services/scripts/scripts.service';
import { GameService } from '../services/game/game.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  host: {class: 'vertical-flex'},
})
export class HomeComponent implements OnInit {

  pythonExecutable: string | undefined;
  scripts: string[] = [];
  selectedPlayers: string[] = [undefined, undefined, undefined, undefined];

  constructor(private scriptsService: ScriptsService,
    private gameService: GameService) { }

  ngOnInit() {
    this.loadAIScripts();
  }

  onPlayerSelect(playerName: string, playerNumber: number) {
    this.selectedPlayers[playerNumber - 1] = playerName;
  }

  addAIScript() {
    // TODO: show toast with result
    this.scriptsService.addScript$().subscribe((wasAdded: boolean) => {
      console.log(`Adding AI script finished with result: ${wasAdded}.`);
      this.loadAIScripts();
    });
  }

  addAIFolder() {
    // TODO: show toast with result
    this.scriptsService.addFolder$().subscribe((wasAdded: boolean) => {
      console.log(`Adding AI folder finished with result: ${wasAdded}.`);
      this.loadAIScripts();
    });
  }

  selectPythonExecutable() {
    this.scriptsService.setPythonExecutable$().subscribe((path: string | undefined) => {
      console.log(`Setting python path finished: ${path}.`);
      this.pythonExecutable = path;
    });
  }

  startGame() {
    console.log("starting game!");
  }

  loadAIScripts() {
    this.scriptsService.getScripts$().subscribe((result) => {
      this.scripts = result;
    });
  }

}
