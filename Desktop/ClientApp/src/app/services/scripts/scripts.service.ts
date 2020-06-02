import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { take, map } from 'rxjs/operators';

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

  public addScript$(): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl + 'api/ai/addscript', undefined)
      .pipe(
        take(1),
      );
  }

  public addFolder$(): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl + 'api/ai/addfolder', undefined)
      .pipe(
        take(1),
      );
  }

  public setPythonExecutable$(): Observable<string | undefined> {
    return this.http.post<{path: string}>(this.baseUrl + 'api/ai/pythonexecutable', undefined)
      .pipe(
        take(1),
        map(x => {
          if (x) return x.path;
          return undefined;
        }),
      );
  }
}
