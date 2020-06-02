import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ScriptsService } from '../services/scripts/scripts.service';

@Component({
  selector: 'app-scripts',
  templateUrl: './scripts.component.html',
  styleUrls: ['./scripts.component.css']
})
export class ScriptsComponent implements OnInit {

  //@Input() scripts: string[];
  private _scripts;

  get scripts(): string[] {
    return this._scripts;
  }

  @Input()
  set scripts(val: string[]) {
    this._scripts = val;
    if (val && val.length > 0) {
      this.selectScript(val[0]);
    }
  }

  @Output() selection = new EventEmitter<string>();
  selectedScript: string;

  constructor()
  { }

  ngOnInit() {
    if (this.scripts && this.scripts.length > 0) {
      this.selectedScript = this.scripts[0];
      this.selection.next(this.selectedScript);
    }
  }

  selectScript(script: string) {
    this.selectedScript = script;
    this.selection.next(script);
  }
}
