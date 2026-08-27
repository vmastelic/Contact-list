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

  deleteContact(id: number): void {
    this.contactService.deleteContact(id).subscribe({
      next: () => {
        console.log(`Contact with id ${id} deleted successfully.`);
        this.contacts.set(this.contacts().filter(contact => contact.id !== id));
      },
      error: (error) => {
        console.error(`Error deleting contact with id ${id}:`, error);
      }
    });
  }

}
