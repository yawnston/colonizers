import { Component, Input, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { GameEndInfo, PlayerEndInfo } from '../services/models/gamestate';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-game-end',
  templateUrl: './game-end.component.html',
  styleUrls: ['./game-end.component.css']
})
export class GameEndComponent implements AfterViewInit {
  @Input() gameEndInfo: GameEndInfo;
  @ViewChild('content', { static: true }) content: ElementRef;
  closeResult: string;

  constructor(private modalService: NgbModal, private router: Router) {
  }

  sortPlayersByRanking(players: PlayerEndInfo[]): PlayerEndInfo[] {
    return players.sort((a, b) => {
      if (a.ranking < b.ranking) return -1;
      if (a.ranking > b.ranking) return 1;
      return 0;
    });
  }

  open(content) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}.`;
    });
  }

  closeAndNavigateAway() {
    this.modalService.dismissAll('OK and navigate');
    this.router.navigateByUrl('/');
  }

  ngAfterViewInit() {
    this.open(this.content);
  }
}
