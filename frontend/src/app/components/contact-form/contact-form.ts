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
  Tag,
  CreateContactRequest,
  UpdateContactRequest
} from '../../models/contact';

import {
  ContactService
} from '../../services/contact.service';

import {
  TagService
} from '../../services/tag.service';


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

  private tagService =
    inject(TagService);

  private formBuilder =
    inject(FormBuilder);


  // null = creating a new contact
  @Input()
  contact: Contact | null = null;


  // All tags from the database
  @Input()
  availableTags: Tag[] = [];


  // Emits after a contact is created or updated
  @Output()
  saved =
    new EventEmitter<Contact>();


  // Emits when the user closes the form
  @Output()
  cancelled =
    new EventEmitter<void>();


  // Emits when a new tag is created
  @Output()
  tagCreated =
    new EventEmitter<Tag>();


  serverError =
    signal('');


  selectedTagIds =
    signal<number[]>([]);


  newTagName =
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
        this.formBuilder
          .array<PhoneFormGroup>([]),

      emailAddresses:
        this.formBuilder
          .array<EmailFormGroup>([])
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

    this.newTagName.set('');

    this.contactForm.reset();

    this.phoneNumbers.clear();

    this.emailAddresses.clear();

    this.selectedTagIds.set([]);


    // ADD mode
    if (this.contact === null) {
      return;
    }


    // EDIT mode
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


    // Select tags already assigned to this contact
    this.selectedTagIds.set(
      this.contact.tags.map(
        tag => tag.id
      )
    );


    // Existing phone numbers
    this.contact.phoneNumbers
      .forEach(phone => {

        this.phoneNumbers.push(
          this.createPhoneGroup(
            phone.type,
            phone.value
          )
        );
      });


    // Existing email addresses
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


  // -------------------------
  // TAG SELECTION
  // -------------------------

  isTagSelected(
    tagId: number
  ): boolean {

    return this.selectedTagIds()
      .includes(tagId);
  }


  toggleTag(
    tagId: number
  ): void {

    const currentIds =
      this.selectedTagIds();


    if (currentIds.includes(tagId)) {

      this.selectedTagIds.set(
        currentIds.filter(
          id => id !== tagId
        )
      );

    } else {

      this.selectedTagIds.set([
        ...currentIds,
        tagId
      ]);
    }
  }


  // -------------------------
  // NEW TAG
  // -------------------------

  onNewTagInput(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;

    this.newTagName.set(
      input.value
    );
  }


  createTag(): void {

    this.serverError.set('');


    const name =
      this.newTagName().trim();


    if (!name) {

      this.serverError.set(
        'Please enter a tag name.'
      );

      return;
    }


    this.tagService
      .createTag(name)
      .subscribe({

        next: (createdTag) => {

          // Tell parent that a new tag exists
          this.tagCreated.emit(
            createdTag
          );


          // Automatically select the new tag
          this.selectedTagIds.update(
            ids => {

              if (
                ids.includes(
                  createdTag.id
                )
              ) {
                return ids;
              }


              return [
                ...ids,
                createdTag.id
              ];
            }
          );


          // Clear input
          this.newTagName.set('');
        },


        error: (error) => {

          console.error(
            'Error creating tag:',
            error
          );


          // Backend returns 409
          // when a tag already exists
          if (error.status === 409) {

            this.serverError.set(
              'A tag with this name already exists.'
            );

          } else {

            this.serverError.set(
              'Could not create the tag.'
            );
          }
        }
      });
  }


  // -------------------------
  // SAVE CONTACT
  // -------------------------

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
      this.contactForm
        .getRawValue();


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


      tagIds:
        this.selectedTagIds()
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
      this.contactForm
        .getRawValue();


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
        this.selectedTagIds()
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