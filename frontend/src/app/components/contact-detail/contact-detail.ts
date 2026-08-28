import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';

import {
  Contact
} from '../../models/contact';

import {
  ContactService
} from '../../services/contact.service';

@Component({
  selector: 'app-contact-detail',
  standalone: true,

  imports: [
    RouterLink
  ],

  templateUrl: './contact-detail.html',
  styleUrl: './contact-detail.css'
})
export class ContactDetailComponent
  implements OnInit {

  private route =
    inject(ActivatedRoute);

  private contactService =
    inject(ContactService);

  contact =
    signal<Contact | null>(null);

  loading =
    signal(true);

  notFound =
    signal(false);

  ngOnInit(): void {

    const id =
      Number(
        this.route.snapshot.paramMap.get('id')
      );

    if (
      !Number.isInteger(id) ||
      id <= 0
    ) {

      this.loading.set(false);

      this.notFound.set(true);

      return;
    }

    this.contactService
      .getContactById(id)
      .subscribe({

        next: (contact) => {

          this.contact.set(
            contact
          );

          this.loading.set(
            false
          );
        },

        error: (error) => {

          console.error(
            'Error loading contact:',
            error
          );

          this.loading.set(
            false
          );

          this.notFound.set(
            true
          );
        }
      });
  }
}