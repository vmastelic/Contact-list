# Contact List

A full-stack contact management application built as a qualification project for a developer internship.

The application allows a single user to manage contacts, phone numbers, email addresses and tags through a REST API and an Angular single-page application.

## Features

### Contact Management

- Create contacts
- View contacts
- Edit contacts
- Delete contacts
- Store first name and last name
- Store address information
- Store notes
- Confirmation before deleting a contact

### Phone Numbers

Each contact can have multiple phone numbers.

Each phone number contains:

- Type
- Value

Examples:

- Mobile
- Work
- Home

### Email Addresses

Each contact can have multiple email addresses.

Each email address contains:

- Type
- Value

Examples:

- Personal
- Work

### Tags

Contacts can have multiple tags.

Supported functionality includes:

- Create new tags
- Assign multiple tags to a contact
- Remove tags from a contact
- Filter contacts by tag
- Prevent creation of duplicate tag names
- Search contacts by tag

### Search

Search is performed on the server side.

Contacts can be searched by:

- First name
- Last name
- Phone number
- Email address
- Tag

### Sorting

Contacts can be sorted on the server side by:

- First name
- Last name

Supported directions:

- Ascending
- Descending

### Pagination

Pagination is performed on the server side.

The user can:

- Move to the previous page
- Move to the next page
- Select the number of contacts per page
- See the current page and total number of pages

### Validation

The application contains validation on both the frontend and backend.

Examples include:

- Required first name
- Required last name
- Email validation
- Phone number validation
- Maximum field lengths
- Duplicate tag prevention

### Routing

Angular routing is used for navigation.

Examples:

```text
/contacts
/contacts/5
```

A contact can be opened on its own URL and browser Back/Forward navigation is supported.

## Technologies

### Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- PostgreSQL
- Npgsql
- Swagger / OpenAPI
- Code First migrations

### Frontend

- Angular
- TypeScript
- Angular Signals
- Reactive Forms
- Angular Router
- HttpClient
- HTML
- CSS

## Project Structure

```text
contact-list/
├── backend/
│   └── ContactManager.Api/
│       ├── Controllers/
│       ├── Data/
│       ├── DTOs/
│       ├── Entities/
│       ├── Migrations/
│       ├── Services/
│       └── Program.cs
│
└── frontend/
    └── src/
        └── app/
            ├── components/
            │   ├── contact-list/
            │   ├── contact-form/
            │   └── contact-detail/
            ├── models/
            ├── services/
            ├── app.ts
            ├── app.html
            ├── app.config.ts
            └── app.routes.ts
```

## Database Model

The main entities are:

### Contact

Contains:

- Id
- FirstName
- LastName
- Street
- City
- PostalCode
- Country
- Note

A contact can have multiple:

- Phone numbers
- Email addresses
- Tags

### PhoneNumber

Contains:

- Id
- Type
- Value
- ContactId

### EmailAddress

Contains:

- Id
- Type
- Value
- ContactId

### Tag

Contains:

- Id
- Name

Contacts and tags have a many-to-many relationship.

## REST API

The backend exposes REST endpoints for contact and tag management.

### Contacts

```text
GET    /api/Contacts
GET    /api/Contacts/{id}
POST   /api/Contacts
PUT    /api/Contacts/{id}
DELETE /api/Contacts/{id}
```

### Search, Sorting and Pagination

```text
GET /api/Contacts/search
```

Supported query parameters:

```text
search
page
pageSize
sortBy
sortDirection
tagId
```

Example:

```text
GET /api/Contacts/search?search=john&page=1&pageSize=5&sortBy=lastName&sortDirection=asc
```

### Tag Filter

```text
GET /api/Contacts/filter?tagId=1
```

### Tags

```text
GET    /api/Tags
POST   /api/Tags
DELETE /api/Tags/{id}
```

## Getting Started

### Prerequisites

Install:

- .NET SDK
- Node.js and npm
- PostgreSQL
- Angular CLI

## Backend Setup

Navigate to the backend project:

```bash
cd backend/ContactManager.Api
```

Configure the PostgreSQL connection string using:

```text
ConnectionStrings:DefaultConnection
```

Example connection string:

```text
Host=localhost;Port=5432;Database=contact_manager;Username=postgres;Password=your_password
```

Apply Entity Framework migrations:

```bash
dotnet ef database update
```

Run the backend:

```bash
dotnet run
```

Swagger can be used to inspect and test the API while the backend is running.

## Frontend Setup

Navigate to the frontend:

```bash
cd frontend
```

Install dependencies:

```bash
npm install
```

Start the Angular development server:

```bash
npm start
```

The frontend is available at:

```text
http://localhost:4200
```

The backend API is configured to run locally at:

```text
https://localhost:7090
```

## Initial Data

The application contains database seed data for development and testing.

The seed includes:

- 20 sample contacts
- Multiple phone numbers
- Multiple email addresses
- Multiple tags

Example tags include:

- Friend
- Work
- Family
- Client
- Supplier

The seed data makes it possible to test:

- Search
- Sorting
- Pagination
- Tag filtering
- Contact details

## Example Workflow

A user can:

1. Open the contact list.
2. Search for a contact.
3. Filter contacts by tag.
4. Sort contacts by first or last name.
5. Navigate through paginated results.
6. Create a new contact.
7. Add multiple phone numbers.
8. Add multiple email addresses.
9. Assign multiple tags.
10. Create a new tag.
11. Edit an existing contact.
12. Open a contact detail page.
13. Delete a contact.

## Architecture

The backend follows a simple layered structure:

```text
Controller
   ↓
Service
   ↓
Entity Framework Core
   ↓
PostgreSQL
```

The Angular frontend is divided into components and services:

```text
ContactListComponent
ContactFormComponent
ContactDetailComponent
        ↓
ContactService / TagService
        ↓
ASP.NET Core REST API
```

This keeps UI logic, API communication and backend business logic separated.

## Development

The project was developed incrementally with Git.

Main development areas included:

- REST API implementation
- Entity Framework Code First
- Database migrations
- DTO mapping
- CRUD operations
- Server-side search
- Server-side sorting
- Server-side pagination
- Angular Reactive Forms
- Angular Signals
- Dynamic phone and email fields
- Tag management
- Routing
- Validation
- Error handling

## Status

The core application requirements are implemented and functional.

Possible future improvements include:

- Automated tests
- CSV import/export
- Duplicate contact detection
- Contact archiving / soft delete
- Batch operations
- Improved global exception handling
- Authentication and multiple users
