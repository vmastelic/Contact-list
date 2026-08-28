import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams
} from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Contact,
  CreateContactRequest,
  UpdateContactRequest,
  PagedResult
} from '../models/contact';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  private apiUrl =
    'https://localhost:7090/api/Contacts';

  constructor(
    private http: HttpClient
  ) {
  }

  getContacts(): Observable<Contact[]> {

    return this.http.get<Contact[]>(
      this.apiUrl
    );
  }

  addContact(
    contact: CreateContactRequest
  ): Observable<Contact> {

    return this.http.post<Contact>(
      this.apiUrl,
      contact
    );
  }

  updateContact(
    id: number,
    contact: UpdateContactRequest
  ): Observable<Contact> {

    return this.http.put<Contact>(
      `${this.apiUrl}/${id}`,
      contact
    );
  }

  deleteContact(
    id: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

  searchContacts(
    search: string,
    page: number,
    pageSize: number,
    sortBy: string,
    sortDirection: string,
    tagId: number | null
  ): Observable<PagedResult<Contact>> {

    let params =
      new HttpParams()
        .set(
          'page',
          page.toString()
        )
        .set(
          'pageSize',
          pageSize.toString()
        )
        .set(
          'sortBy',
          sortBy
        )
        .set(
          'sortDirection',
          sortDirection
        );

    if (search.trim()) {

      params = params.set(
        'search',
        search.trim()
      );
    }

    if (tagId !== null) {

      params = params.set(
        'tagId',
        tagId.toString()
      );
    }

    return this.http.get<
      PagedResult<Contact>
    >(
      `${this.apiUrl}/search`,
      {
        params
      }
    );
  }

  filterContacts(
    tagId: number
  ): Observable<Contact[]> {

    return this.http.get<Contact[]>(
      `${this.apiUrl}/filter`,
      {
        params: {
          tagId
        }
      }
    );
  }
}