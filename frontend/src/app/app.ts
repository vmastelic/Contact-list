import { Component, OnInit, signal, inject } from '@angular/core';
import { ContactService } from './services/contact.service';
import { Contact } from './models/contact';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  FormGroup
} from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [ReactiveFormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  contacts = signal<Contact[]>([]);

  private contactService = inject(ContactService);
  private formBuilder = inject(FormBuilder);

  showAddForm = false;

  contactForm = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    street: [''],
    city: [''],
    postalCode: [''],
    country: [''],
    note: [''],

    phoneNumbers: this.formBuilder.array<FormGroup>([]),
    emailAddresses: this.formBuilder.array<FormGroup>([]),
    tags: this.formBuilder.array<FormGroup>([])
  });

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

  get phoneNumbers() {
    return this.contactForm.controls.phoneNumbers;
  }

  get emailAddresses() {
    return this.contactForm.controls.emailAddresses;
  }

  addPhoneNumber(): void {
    const phone = this.formBuilder.group({
      type: ['', Validators.required],
      value: ['', Validators.required]
    });

    this.phoneNumbers.push(phone);
  }

  removePhoneNumber(index: number): void {
    this.phoneNumbers.removeAt(index);
  }

  addEmailAddress(): void {
    const email = this.formBuilder.group({
      type: ['', Validators.required],
      value: ['', [Validators.required, Validators.email]]
    });

    this.emailAddresses.push(email);
  }

  removeEmailAddress(index: number): void {
    this.emailAddresses.removeAt(index);
  }

  deleteContact(id: number): void {
    this.contactService.deleteContact(id).subscribe({
      next: () => {
        console.log(`Contact with id ${id} deleted successfully.`);

        this.contacts.set(
          this.contacts().filter(contact => contact.id !== id)
        );
      },
      error: (error) => {
        console.error(
          `Error deleting contact with id ${id}:`,
          error
        );
      }
    });
  }

  addContact(): void {
    if (this.contactForm.invalid) {
      console.error(
        'Form is invalid. Please fill in all required fields.'
      );
      return;
    }

    const formValue = this.contactForm.getRawValue();

    const newContact: Contact = {
      id: 0,

      firstName: formValue.firstName ?? '',
      lastName: formValue.lastName ?? '',
      street: formValue.street ?? '',
      city: formValue.city ?? '',
      postalCode: formValue.postalCode ?? '',
      country: formValue.country ?? '',
      note: formValue.note ?? '',

      phoneNumbers: formValue.phoneNumbers.map(phone => ({
        type: phone['type'] ?? '',
        value: phone['value'] ?? ''
      })),

      emailAddresses: formValue.emailAddresses.map(email => ({
        type: email['type'] ?? '',
        value: email['value'] ?? ''
      })),

      tags: []
    };

    this.contactService.addContact(newContact).subscribe({
      next: (addedContact) => {
        console.log(
          'Contact added successfully:',
          addedContact
        );

        this.contacts.set([
          ...this.contacts(),
          addedContact
        ]);

        this.contactForm.reset();

        this.phoneNumbers.clear();
        this.emailAddresses.clear();

        this.showAddForm = false;
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