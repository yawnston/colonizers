import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { GameComponent } from './game/game.component';
import { PlayerComponent } from './player/player.component';
import { BoardComponent } from './board/board.component';
import { GameEndComponent } from './game-end/game-end.component'
import { ModuleComponent } from './module/module.component';
import { ScriptsComponent } from './scripts/scripts.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    GameComponent,
    PlayerComponent,
    BoardComponent,
    GameEndComponent,
    ModuleComponent,
    ScriptsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'game', component: GameComponent },
    ]),
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
