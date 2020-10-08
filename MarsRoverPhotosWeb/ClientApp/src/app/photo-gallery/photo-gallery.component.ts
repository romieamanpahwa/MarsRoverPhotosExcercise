import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DownloadService } from "../services/download.service";

@Component({
  selector: 'app-photo-gallery',
  templateUrl: './photo-gallery.component.html'
})
export class PhotoGalleryComponent implements OnInit {
  public photos: Photo[];
  readonly apiURL = "http://localhost:54515/api/marsroverphotos/";

  constructor(private http: HttpClient, private downloadService: DownloadService) { }

  ngOnInit() {
    this.http.get<Photo[]>(`${this.apiURL}photos/2017-02-27`).subscribe(response => {   
      this.photos = response;
      
      /** TBD: store photos locally.
       * downloadService.download(): does the following:
       * 1. get memoryStream/blob of img_src which is the URL of an image that's on external domain
       * 2. using FileSaver.js convert  blob into fileStream using saveAs()

      let photosCount = response.length;
      if (photosCount > 0) {
        for (let loopCtr = 0; loopCtr < photosCount; loopCtr++) {
          this.downloadService.download(response[loopCtr].img_src, `${response[loopCtr].id}`);
        }
      }
       * */
       
    }, error => {
      console.log(error);
    });
  }

}

interface Photo {
  id: string;
  img_src: string;
}
