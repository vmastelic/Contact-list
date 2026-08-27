export interface PhoneNumber {
  type: string;
  value: string;
}

export interface EmailAddress {
  type: string;
  value: string;
}

export interface Tag {
  id: number;
  name: string;
}

export interface Contact {
  id: number;
  firstName: string;
  lastName: string;
  street: string;
  city: string;
  postalCode: string;
  country: string;
  note: string;

  phoneNumbers: PhoneNumber[];
  emailAddresses: EmailAddress[];
  tags: Tag[];
}