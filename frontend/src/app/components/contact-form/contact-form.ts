import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  inject,
  signal
} from '@angular/core';

import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  Contact,
  CreateContactRequest,
  UpdateContactRequest
} from '../../models/contact';

import {
  ContactService
} from '../../services/contact.service';


type PhoneFormGroup = FormGroup<{
  type: FormControl<string>;
  value: FormControl<string>;
}>;


type EmailFormGroup = FormGroup<{
  type: FormControl<string>;
  value: FormControl<string>;
}>;


@Component({
  selector: 'app-contact-form',
  standalone: true,

  imports: [
    ReactiveFormsModule
  ],

  templateUrl: './contact-form.html',
  styleUrl: './contact-form.css'
})
export class ContactFormComponent
  implements OnChanges {

  private contactService =
    inject(ContactService);

  private formBuilder =
    inject(FormBuilder);


  @Input()
  contact: Contact | null = null;


  @Output()
  saved =
    new EventEmitter<Contact>();


  @Output()
  cancelled =
    new EventEmitter<void>();


  serverError =
    signal('');


  contactForm =
    this.formBuilder.nonNullable.group({

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


  ngOnChanges(
    changes: SimpleChanges
  ): void {

    if (changes['contact']) {
      this.populateForm();
    }
  }


  get isEditing(): boolean {
    return this.contact !== null;
  }


  get phoneNumbers():
    FormArray<PhoneFormGroup> {

    return this.contactForm
      .controls.phoneNumbers;
  }


  get emailAddresses():
    FormArray<EmailFormGroup> {

    return this.contactForm
      .controls.emailAddresses;
  }


  private populateForm(): void {

    this.serverError.set('');

    this.contactForm.reset();

    this.phoneNumbers.clear();

    this.emailAddresses.clear();


    if (this.contact === null) {
      return;
    }


    this.contactForm.patchValue({

      firstName:
        this.contact.firstName,

      lastName:
        this.contact.lastName,

      street:
        this.contact.street,

      city:
        this.contact.city,

      postalCode:
        this.contact.postalCode,

      country:
        this.contact.country,

      note:
        this.contact.note
    });


    this.contact.phoneNumbers
      .forEach(phone => {

        this.phoneNumbers.push(
          this.createPhoneGroup(
            phone.type,
            phone.value
          )
        );
      });


    this.contact.emailAddresses
      .forEach(email => {

        this.emailAddresses.push(
          this.createEmailGroup(
            email.type,
            email.value
          )
        );
      });
  }


  private createPhoneGroup(
    type: string = '',
    value: string = ''
  ): PhoneFormGroup {

    return this.formBuilder
      .nonNullable
      .group({

        type: [
          type,
          Validators.required
        ],

        value: [
          value,
          [
            Validators.required,

            Validators.pattern(
              /^[0-9+\s()-]{6,20}$/
            )
          ]
        ]
      });
  }


  private createEmailGroup(
    type: string = '',
    value: string = ''
  ): EmailFormGroup {

    return this.formBuilder
      .nonNullable
      .group({

        type: [
          type,
          Validators.required
        ],

        value: [
          value,
          [
            Validators.required,
            Validators.email
          ]
        ]
      });
  }


  addPhoneNumber(): void {

    this.phoneNumbers.push(
      this.createPhoneGroup()
    );
  }


  removePhoneNumber(
    index: number
  ): void {

    this.phoneNumbers.removeAt(
      index
    );
  }


  addEmailAddress(): void {

    this.emailAddresses.push(
      this.createEmailGroup()
    );
  }


  removeEmailAddress(
    index: number
  ): void {

    this.emailAddresses.removeAt(
      index
    );
  }


  saveContact(): void {

    this.serverError.set('');


    if (this.contactForm.invalid) {

      this.contactForm
        .markAllAsTouched();

      return;
    }


    if (this.contact === null) {

      this.createContact();

    } else {

      this.updateContact();
    }
  }


  private createContact(): void {

    const formValue =
      this.contactForm.getRawValue();


    const request:
      CreateContactRequest = {

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
        formValue.phoneNumbers
          .map(phone => ({

            type:
              phone.type,

            value:
              phone.value
          })),


      emailAddresses:
        formValue.emailAddresses
          .map(email => ({

            type:
              email.type,

            value:
              email.value
          })),


      tagIds: []
    };


    this.contactService
      .addContact(request)
      .subscribe({

        next: (createdContact) => {

          this.saved.emit(
            createdContact
          );
        },


        error: (error) => {

          console.error(
            'Error adding contact:',
            error
          );


          this.serverError.set(
            'Could not save the contact.'
          );
        }
      });
  }


  private updateContact(): void {

    const currentContact =
      this.contact;


    if (currentContact === null) {
      return;
    }


    const formValue =
      this.contactForm.getRawValue();


    const request:
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
        formValue.phoneNumbers
          .map(phone => ({

            type:
              phone.type,

            value:
              phone.value
          })),


      emailAddresses:
        formValue.emailAddresses
          .map(email => ({

            type:
              email.type,

            value:
              email.value
          })),


      tagIds:
        currentContact.tags
          .map(tag => tag.id)
    };


    this.contactService
      .updateContact(
        currentContact.id,
        request
      )
      .subscribe({

        next: (updatedContact) => {

          this.saved.emit(
            updatedContact
          );
        },


        error: (error) => {

          console.error(
            'Error updating contact:',
            error
          );


          this.serverError.set(
            'Could not update the contact.'
          );
        }
      });
  }


  cancel(): void {

    this.cancelled.emit();
  }
}