import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { saveAs } from 'file-saver';
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class DownloadService {

  constructor(
    private http: HttpClient
  ) { }

  download(url: string, filename?: string) {
    return this.http.get(url, {
      responseType: 'blob',
      headers: { 'Access-Control-Allow-Origin': 'http://localhost:54515' }
    })
      .subscribe(
        blob => {
          console.log('1');
          saveAs(blob, filename)
        });
  }
}

