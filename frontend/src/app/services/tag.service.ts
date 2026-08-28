import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Tag } from '../models/contact';

@Injectable({
  providedIn: 'root'
})
export class TagService {

  private apiUrl =
    'https://localhost:7090/api/Tags';

  constructor(
    private http: HttpClient
  ) {
  }

  getTags(): Observable<Tag[]> {

    return this.http.get<Tag[]>(
      this.apiUrl
    );
  }
}