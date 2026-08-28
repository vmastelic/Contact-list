import {
  Component,
  OnInit,
  signal,
  inject
} from '@angular/core';

import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormBuilder
} from '@angular/forms';

import { ContactService } from './services/contact.service';

import {
  Contact,
  CreateContactRequest,
  UpdateContactRequest
} from './models/contact';


type PhoneFormGroup = FormGroup<{
  type: FormControl<string>;
  value: FormControl<string>;
}>;


type EmailFormGroup = FormGroup<{
  type: FormControl<string>;
  value: FormControl<string>;
}>;


@Component({
  selector: 'app-root',

  imports: [
    ReactiveFormsModule
  ],

  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  private contactService = inject(ContactService);
  private formBuilder = inject(FormBuilder);

  contacts = signal<Contact[]>([]);

  showAddForm = false;

  editingContactId: number | null = null;

  editingTagIds: number[] = [];

  contactForm = this.formBuilder.nonNullable.group({

    firstName: [
      '',
      [
        Validators.required,
        Validators.maxLength(100)
      ]
    ],

    lastName: [
      '',
      [
        Validators.required,
        Validators.maxLength(100)
      ]
    ],

    street: [
      '',
      Validators.maxLength(200)
    ],

    city: [
      '',
      Validators.maxLength(100)
    ],

    postalCode: [
      '',
      Validators.maxLength(20)
    ],

    country: [
      '',
      Validators.maxLength(100)
    ],

    note: [
      '',
      Validators.maxLength(1000)
    ],

    phoneNumbers:
      this.formBuilder.array<PhoneFormGroup>([]),

    emailAddresses:
      this.formBuilder.array<EmailFormGroup>([])
  });

  ngOnInit(): void {
    this.loadContacts();
  }

  loadContacts(): void {

    this.contactService
      .getContacts()
      .subscribe({

        next: (data) => {

          console.log(
            'Contacts received:',
            data
          );

          this.contacts.set(data);
        },

        error: (error) => {

          console.error(
            'Error loading contacts:',
            error
          );
        }
      });
  }

  get phoneNumbers(): FormArray<PhoneFormGroup> {
    return this.contactForm.controls.phoneNumbers;
  }


  get emailAddresses(): FormArray<EmailFormGroup> {
    return this.contactForm.controls.emailAddresses;
  }

  openAddForm(): void {

    this.editingContactId = null;
    this.editingTagIds = [];

    this.contactForm.reset();

    this.phoneNumbers.clear();
    this.emailAddresses.clear();

    this.showAddForm = true;
  }

  cancelForm(): void {

    this.showAddForm = false;

    this.editingContactId = null;
    this.editingTagIds = [];

    this.contactForm.reset();

    this.phoneNumbers.clear();
    this.emailAddresses.clear();
  }

  addPhoneNumber(): void {

    const phone =
      this.formBuilder.nonNullable.group({

        type: [
          '',
          Validators.required
        ],

        value: [
          '',
          [
            Validators.required,

            Validators.pattern(
              /^[0-9+\s()-]{6,20}$/
            )
          ]
        ]
      });

    this.phoneNumbers.push(phone);
  }

  removePhoneNumber(index: number): void {

    this.phoneNumbers.removeAt(index);
  }

  addEmailAddress(): void {

    const email =
      this.formBuilder.nonNullable.group({

        type: [
          '',
          Validators.required
        ],

        value: [
          '',
          [
            Validators.required,
            Validators.email
          ]
        ]
      });

    this.emailAddresses.push(email);
  }

  removeEmailAddress(index: number): void {

    this.emailAddresses.removeAt(index);
  }

  saveContact(): void {

    if (this.editingContactId === null) {

      this.addContact();

    } else {

      this.updateContact();
    }
  }

  addContact(): void {

    if (this.contactForm.invalid) {

      this.contactForm.markAllAsTouched();

      return;
    }


    const formValue =
      this.contactForm.getRawValue();


    const newContact: CreateContactRequest = {

      firstName:
        formValue.firstName,

      lastName:
        formValue.lastName,

      street:
        formValue.street,

      city:
        formValue.city,

      postalCode:
        formValue.postalCode,

      country:
        formValue.country,

      note:
        formValue.note,


      phoneNumbers:
        formValue.phoneNumbers.map(phone => ({

          type: phone.type,

          value: phone.value
        })),


      emailAddresses:
        formValue.emailAddresses.map(email => ({

          type: email.type,

          value: email.value
        })),


      tagIds: []
    };


    this.contactService
      .addContact(newContact)
      .subscribe({

        next: (addedContact) => {

          console.log(
            'Contact added successfully:',
            addedContact
          );


          this.contacts.update(
            contacts => [
              ...contacts,
              addedContact
            ]
          );


          this.cancelForm();
        },


        error: (error) => {

          console.error(
            'Error adding contact:',
            error
          );

          console.error(
            'Backend validation errors:',
            error.error?.errors
          );
        }
      });
  }

  editContact(contact: Contact): void {

    this.editingContactId =
      contact.id;


    this.editingTagIds =
      contact.tags.map(
        tag => tag.id
      );


    this.contactForm.reset();

    this.phoneNumbers.clear();
    this.emailAddresses.clear();

    this.contactForm.patchValue({

      firstName:
        contact.firstName,

      lastName:
        contact.lastName,

      street:
        contact.street,

      city:
        contact.city,

      postalCode:
        contact.postalCode,

      country:
        contact.country,

      note:
        contact.note
    });

    contact.phoneNumbers.forEach(phone => {

      const phoneGroup =
        this.formBuilder.nonNullable.group({

          type: [
            phone.type,
            Validators.required
          ],

          value: [
            phone.value,
            [
              Validators.required,

              Validators.pattern(
                /^[0-9+\s()-]{6,20}$/
              )
            ]
          ]
        });


      this.phoneNumbers.push(
        phoneGroup
      );
    });

    contact.emailAddresses.forEach(email => {

      const emailGroup =
        this.formBuilder.nonNullable.group({

          type: [
            email.type,
            Validators.required
          ],

          value: [
            email.value,
            [
              Validators.required,
              Validators.email
            ]
          ]
        });


      this.emailAddresses.push(
        emailGroup
      );
    });


    this.showAddForm = true;
  }

  updateContact(): void {

    if (
      this.contactForm.invalid ||
      this.editingContactId === null
    ) {

      this.contactForm.markAllAsTouched();

      return;
    }


    const formValue =
      this.contactForm.getRawValue();


    const updatedContact:
      UpdateContactRequest = {

      firstName:
        formValue.firstName,

      lastName:
        formValue.lastName,

      street:
        formValue.street,

      city:
        formValue.city,

      postalCode:
        formValue.postalCode,

      country:
        formValue.country,

      note:
        formValue.note,


      phoneNumbers:
        formValue.phoneNumbers.map(phone => ({

          type: phone.type,

          value: phone.value
        })),


      emailAddresses:
        formValue.emailAddresses.map(email => ({

          type: email.type,

          value: email.value
        })),


      tagIds:
        this.editingTagIds
    };


    this.contactService
      .updateContact(
        this.editingContactId,
        updatedContact
      )
      .subscribe({

        next: (updated) => {

          console.log(
            'Contact updated successfully:',
            updated
          );


          this.contacts.update(
            contacts =>
              contacts.map(contact =>

                contact.id === updated.id
                  ? updated
                  : contact
              )
          );


          this.cancelForm();
        },


        error: (error) => {

          console.error(
            'Error updating contact:',
            error
          );

          console.error(
            'Backend validation errors:',
            error.error?.errors
          );
        }
      });
  }

  deleteContact(id: number): void {

    this.contactService
      .deleteContact(id)
      .subscribe({

        next: () => {

          console.log(
            `Contact with id ${id} deleted successfully.`
          );


          this.contacts.update(
            contacts =>
              contacts.filter(
                contact =>
                  contact.id !== id
              )
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
}