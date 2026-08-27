import { Component, OnInit, signal } from '@angular/core';
import { ContactService } from './services/contact.service';
import { Contact } from './models/contact';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  contacts = signal<Contact[]>([]);

  constructor(private contactService: ContactService){
  }

  ngOnInit(): void {
  console.log('ngOnInit started');

  this.contactService.getContacts().subscribe({
    next: (data) => {
      console.log('Contacts received:', data);
      this.contacts.set(data);
      },
    error: (error) => {
      console.error('Error loading contacts:', error);
      }
    });
  }
}
