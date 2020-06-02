import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ScriptsService {

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  public getScripts$(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'api/ai/scripts')
      .pipe(
        take(1),
      );
  }
}
