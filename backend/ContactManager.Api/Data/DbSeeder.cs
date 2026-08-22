using ContactManager.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Contacts.AnyAsync())
            {
                return;
            }

            var friendTag = new Tag
            {
                Name = "Friend"
            };

            var workTag = new Tag
            {
                Name = "Work"
            };

            var supplierTag = new Tag
            {
                Name = "Supplier"
            };

            var contacts = new List<Contact>
            {
                new Contact
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Street = "Main Street 15",
                    City = "Split",
                    PostalCode = "21000",
                    Country = "Croatia",
                    Note = "Friend from university",

                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "0911234567"
                        },

                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "021555222"
                        }
                    },

                    EmailAddresses = new List<EmailAddress>
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "john@example.com"
                        },

                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "john@company.com"
                        }
                    },

                    Tags = new List<Tag>
                    {
                        friendTag,
                        workTag
                    }
                },

                new Contact
                {
                    FirstName = "Anna",
                    LastName = "Miller",
                    Street = "Coastal Road 20",
                    City = "Split",
                    PostalCode = "21000",
                    Country = "Croatia",
                    Note = "Business contact",

                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "098222333"
                        }
                    },

                    EmailAddresses = new List<EmailAddress>
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "anna@company.com"
                        }
                    },

                    Tags = new List<Tag>
                    {
                        workTag
                    }
                },

                new Contact
                {
                    FirstName = "Mark",
                    LastName = "Taylor",
                    Street = "Market Street 10",
                    City = "Zagreb",
                    PostalCode = "10000",
                    Country = "Croatia",
                    Note = "Equipment supplier",

                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "099444555"
                        }
                    },

                    EmailAddresses = new List<EmailAddress>
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "mark@supplier.com"
                        }
                    },

                    Tags = new List<Tag>
                    {
                        supplierTag
                    }
                }
            };

            context.Contacts.AddRange(contacts);

            await context.SaveChangesAsync();
        }
    }
}