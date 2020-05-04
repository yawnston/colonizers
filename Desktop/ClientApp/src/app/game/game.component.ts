import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  gameState: any;

  constructor(http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {

    http.get(baseUrl + 'game')
      .pipe(take(1))
      .subscribe(result => {
        this.gameState = JSON.stringify(result, undefined, 1);
    }, error => console.error(error));
  }

  ngOnInit() {
  }

}
