import { Component, OnInit } from '@angular/core';
import { ScriptsService } from '../services/scripts/scripts.service';
import { GameService } from '../services/game/game.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

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
    private gameService: GameService,
    private toastr: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.loadAIScripts();
    this.getPythonExecutable();
    this.gameService.hideInformation = false;
  }

  onPlayerSelect(playerName: string, playerNumber: number) {
    this.selectedPlayers[playerNumber - 1] = playerName;
  }

  addAIScript() {
    this.scriptsService.addScript$().subscribe((wasAdded: boolean) => {
      console.log(`Adding AI script finished with result: ${wasAdded}.`);
      if (wasAdded) {
        this.toastr.success('AI script was successfully copied!', 'SUCCESS');
        this.loadAIScripts();
      }
      else {
        this.toastr.info('The AI script add operation was cancelled.', 'INFO');
      }
    });
  }

  addAIFolder() {
    this.scriptsService.addFolder$().subscribe((wasAdded: boolean) => {
      console.log(`Adding AI folder finished with result: ${wasAdded}.`);
      if (wasAdded) {
        this.toastr.success('AI folder was successfully copied!', 'SUCCESS');
        this.loadAIScripts();
      }
      else {
        this.toastr.info('The AI folder add operation was cancelled.', 'INFO');
      }
    });
  }

  getPythonExecutable() {
    this.scriptsService.getPythonExecutable$().subscribe((path: string | undefined) => {
      this.pythonExecutable = path;
    });
  }

  selectPythonExecutable() {
    this.scriptsService.setPythonExecutable$().subscribe((path: string | undefined) => {
      console.log(`Setting python path finished: ${path}.`);
      this.pythonExecutable = path;
    });
  }

  startGame() {
    console.log("Starting game.");
    if (this.selectedPlayers
      && this.selectedPlayers.every(x => x != undefined
        && this.pythonExecutable)) {
      this.gameService.initGame$(this.selectedPlayers).subscribe((result) => {
        this.router.navigateByUrl('/game');
      });
    }
    else {
      console.log(`Game start validation FAILED: ${JSON.stringify(this.selectedPlayers)}, ${this.pythonExecutable}.`);
      this.toastr.error('Could not start game due to a validation error. Please re-configure the game and try again.', 'ERROR');
    }
  }

  loadAIScripts() {
    this.scriptsService.getScripts$().subscribe((result) => {
      this.scripts = result;
    });
  }

  toggleHidden() {
    this.gameService.hideInformation = !this.gameService.hideInformation;
  }

}
