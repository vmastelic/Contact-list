import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Contact,
  CreateContactRequest,
  UpdateContactRequest
} from '../models/contact';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  private apiUrl = 'https://localhost:7090/api/Contacts';

  constructor(private http: HttpClient) {
  }

  getContacts(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.apiUrl);
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

  deleteContact(id: number): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

  searchContacts(
    search: string,
    page: number = 1,
    pageSize: number = 10,
    sortBy: string = 'lastName',
    sortDirection: string = 'asc'
  ): Observable<Contact[]> {

    return this.http.get<Contact[]>(
      `${this.apiUrl}/search`,
      {
        params: {
          search: search,
          page: page,
          pageSize: pageSize,
          sortBy: sortBy,
          sortDirection: sortDirection
        }
      }
    );
  }
}