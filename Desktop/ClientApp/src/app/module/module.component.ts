import { Component, OnInit, Input } from '@angular/core';
import { Module, Color } from '../services/models/gamestate';

@Component({
  selector: 'app-module',
  templateUrl: './module.component.html',
  styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit {

  @Input() module: Module;

  constructor() { }

  ngOnInit() {
  }

  getClassByColor(color: Color): string {
    switch (color) {
      case Color.red: return "module-red";
      case Color.green: return "module-green";
      case Color.blue: return "module-blue";
      case Color.none: return "module-white";
    }
  }
}
