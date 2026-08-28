import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import {
  Contact,
  Tag
} from '../../models/contact';

import {
  ContactService
} from '../../services/contact.service';

import {
  TagService
} from '../../services/tag.service';

import {
  ContactFormComponent
} from '../contact-form/contact-form';


@Component({
  selector: 'app-contact-list',
  standalone: true,

  imports: [
    ContactFormComponent
  ],

  templateUrl: './contact-list.html',
  styleUrl: './contact-list.css'
})
export class ContactListComponent
  implements OnInit {

  private contactService =
    inject(ContactService);

  private tagService =
    inject(TagService);


  contacts =
    signal<Contact[]>([]);


  tags =
    signal<Tag[]>([]);


  searchText =
    signal('');


  selectedTagId =
    signal<number | null>(null);


  showForm =
    signal(false);


  selectedContact =
    signal<Contact | null>(null);


  ngOnInit(): void {

    this.loadContacts();

    this.loadTags();
  }


  loadContacts(): void {

    this.contactService
      .getContacts()
      .subscribe({

        next: (data) => {

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


  loadTags(): void {

    this.tagService
      .getTags()
      .subscribe({

        next: (data) => {

          this.tags.set(data);
        },


        error: (error) => {

          console.error(
            'Error loading tags:',
            error
          );
        }
      });
  }


  onSearchInput(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.searchText.set(
      input.value
    );
  }


  searchContacts(): void {

    const search =
      this.searchText().trim();


    if (!search) {

      this.loadContacts();

      return;
    }


    this.selectedTagId.set(null);


    this.contactService
      .searchContacts(search)
      .subscribe({

        next: (data) => {

          this.contacts.set(data);
        },


        error: (error) => {

          console.error(
            'Error searching contacts:',
            error
          );
        }
      });
  }


  onTagFilterChange(
    event: Event
  ): void {

    const select =
      event.target as HTMLSelectElement;


    if (!select.value) {

      this.selectedTagId.set(null);

      this.loadContacts();

      return;
    }


    const tagId =
      Number(select.value);


    this.selectedTagId.set(
      tagId
    );


    this.searchText.set('');


    this.filterByTag(
      tagId
    );
  }


  filterByTag(
    tagId: number
  ): void {

    this.contactService
      .filterContacts(tagId)
      .subscribe({

        next: (data) => {

          this.contacts.set(data);
        },


        error: (error) => {

          console.error(
            'Error filtering contacts:',
            error
          );
        }
      });
  }


  openAddForm(): void {

    this.selectedContact.set(null);

    this.showForm.set(true);
  }


  openEditForm(
    contact: Contact
  ): void {

    this.selectedContact.set(
      contact
    );

    this.showForm.set(true);
  }


  closeForm(): void {

    this.showForm.set(false);

    this.selectedContact.set(null);
  }


  onContactSaved(
    contact: Contact
  ): void {

    console.log(
      'Contact saved:',
      contact
    );


    this.closeForm();


    this.searchText.set('');

    this.selectedTagId.set(null);


    this.loadContacts();
  }


  deleteContact(
    id: number
  ): void {

    const confirmed =
      window.confirm(
        'Are you sure you want to delete this contact?'
      );


    if (!confirmed) {
      return;
    }


    this.contactService
      .deleteContact(id)
      .subscribe({

        next: () => {

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
            'Error deleting contact:',
            error
          );
        }
      });
  }
}