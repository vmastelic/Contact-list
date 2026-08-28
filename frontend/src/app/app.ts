import { Component, OnInit, signal } from '@angular/core';
import { ContactService } from './services/contact.service';
import { Contact } from './models/contact';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [FormsModule],
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

  showAddForm = false;
  newContact: Contact = {
    id: 0,
    firstName: '',
    lastName: '',
    phoneNumbers: [],
    emailAddresses: [],
    tags: [],
    street: '',
    city: '',
    postalCode: '',
    country: '',
    note: ''
  };

  addContact(): void {

    this.contactService.addContact(this.newContact).subscribe({

      next: (addedContact) => {

        console.log(
          'Contact added successfully:',
          addedContact
        );

        this.contacts.set([
          ...this.contacts(),
          addedContact
        ]);

        this.showAddForm = false;

        this.newContact = {
          id: 0,
          firstName: '',
          lastName: '',
          phoneNumbers: [],
          emailAddresses: [],
          tags: [],
          street: '',
          city: '',
          postalCode: '',
          country: '',
          note: ''
        };
      },

      error: (error) => {
        console.error(
          'Error adding contact:',
          error
        );
      }
    });
  }

}
