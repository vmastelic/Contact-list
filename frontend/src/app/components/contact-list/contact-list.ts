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

  page =
    signal(1);

  pageSize =
    signal(5);

  totalCount =
    signal(0);

  totalPages =
    signal(0);

  sortBy =
    signal('lastName');

  sortDirection =
    signal('asc');

  showForm =
    signal(false);

  selectedContact =
    signal<Contact | null>(null);

  loading =
    signal(false);

  ngOnInit(): void {

    this.loadContacts();

    this.loadTags();
  }

  loadContacts(): void {

    this.loading.set(true);

    this.contactService
      .searchContacts(
        this.searchText(),
        this.page(),
        this.pageSize(),
        this.sortBy(),
        this.sortDirection(),
        this.selectedTagId()
      )
      .subscribe({

        next: (result) => {

          this.contacts.set(
            result.items
          );

          this.totalCount.set(
            result.totalCount
          );

          this.totalPages.set(
            result.totalPages
          );

          this.page.set(
            result.page
          );

          this.loading.set(false);
        },

        error: (error) => {

          console.error(
            'Error loading contacts:',
            error
          );

          this.loading.set(false);
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

    this.page.set(1);

    this.loadContacts();
  }

  clearSearch(): void {

    this.searchText.set('');

    this.page.set(1);

    this.loadContacts();
  }

  onTagFilterChange(
    event: Event
  ): void {

    const select =
      event.target as HTMLSelectElement;

    if (!select.value) {

      this.selectedTagId.set(
        null
      );

    } else {

      this.selectedTagId.set(
        Number(select.value)
      );
    }

    this.page.set(1);

    this.loadContacts();
  }

  onSortByChange(
    event: Event
  ): void {

    const select =
      event.target as HTMLSelectElement;

    this.sortBy.set(
      select.value
    );

    this.page.set(1);

    this.loadContacts();
  }

  onSortDirectionChange(
    event: Event
  ): void {

    const select =
      event.target as HTMLSelectElement;

    this.sortDirection.set(
      select.value
    );

    this.page.set(1);

    this.loadContacts();
  }

  onPageSizeChange(
    event: Event
  ): void {

    const select =
      event.target as HTMLSelectElement;

    this.pageSize.set(
      Number(select.value)
    );

    this.page.set(1);

    this.loadContacts();
  }

  previousPage(): void {

    if (this.page() <= 1) {
      return;
    }

    this.page.update(
      current => current - 1
    );

    this.loadContacts();
  }

  nextPage(): void {

    if (
      this.page() >=
      this.totalPages()
    ) {
      return;
    }

    this.page.update(
      current => current + 1
    );

    this.loadContacts();
  }

  openAddForm(): void {

    this.selectedContact.set(
      null
    );

    this.showForm.set(
      true
    );
  }

  openEditForm(
    contact: Contact
  ): void {

    this.selectedContact.set(
      contact
    );

    this.showForm.set(
      true
    );
  }

  closeForm(): void {

    this.showForm.set(
      false
    );

    this.selectedContact.set(
      null
    );
  }

  onTagCreated(
    tag: Tag
  ): void {

    this.tags.update(
      currentTags => {

        const exists =
          currentTags.some(
            current =>
              current.id === tag.id
          );

        if (exists) {
          return currentTags;
        }

        return [
          ...currentTags,
          tag
        ].sort(
          (a, b) =>
            a.name.localeCompare(
              b.name
            )
        );
      }
    );
  }

  onContactSaved(
    contact: Contact
  ): void {

    console.log(
      'Contact saved:',
      contact
    );

    this.closeForm();

    this.loadContacts();

    this.loadTags();
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

    const onlyContactOnPage =
      this.contacts().length === 1;

    this.contactService
      .deleteContact(id)
      .subscribe({

        next: () => {

          if (
            onlyContactOnPage &&
            this.page() > 1
          ) {

            this.page.update(
              current => current - 1
            );
          }

          this.loadContacts();
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