using ContactManager.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context
        )
        {
            var friendTag =
                await GetOrCreateTagAsync(
                    context,
                    "Friend"
                );

            var workTag =
                await GetOrCreateTagAsync(
                    context,
                    "Work"
                );

            var familyTag =
                await GetOrCreateTagAsync(
                    context,
                    "Family"
                );

            var clientTag =
                await GetOrCreateTagAsync(
                    context,
                    "Client"
                );

            var supplierTag =
                await GetOrCreateTagAsync(
                    context,
                    "Supplier"
                );

            await context.SaveChangesAsync();

            if (await context.Contacts.AnyAsync())
            {
                return;
            }

            var contacts = new List<Contact>
            {
                new Contact
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Street = "15 King Street",
                    City = "London",
                    PostalCode = "SW1A 1AA",
                    Country = "United Kingdom",
                    Note = "University friend",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900101"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "john.smith@example.com"
                        }
                    },
                    Tags =
                    {
                        friendTag
                    }
                },

                new Contact
                {
                    FirstName = "Emily",
                    LastName = "Johnson",
                    Street = "42 Oxford Road",
                    City = "Manchester",
                    PostalCode = "M1 1AE",
                    Country = "United Kingdom",
                    Note = "Marketing colleague",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900102"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "emily.johnson@example.com"
                        }
                    },
                    Tags =
                    {
                        workTag
                    }
                },

                new Contact
                {
                    FirstName = "Michael",
                    LastName = "Brown",
                    Street = "8 Queen Street",
                    City = "Birmingham",
                    PostalCode = "B1 1BB",
                    Country = "United Kingdom",
                    Note = "Software client",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900103"
                        },
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 121 555 0103"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "michael.brown@example.com"
                        }
                    },
                    Tags =
                    {
                        clientTag
                    }
                },

                new Contact
                {
                    FirstName = "Sarah",
                    LastName = "Wilson",
                    Street = "21 Park Avenue",
                    City = "Liverpool",
                    PostalCode = "L1 8JQ",
                    Country = "United Kingdom",
                    Note = "Family member",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900104"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "sarah.wilson@example.com"
                        }
                    },
                    Tags =
                    {
                        familyTag
                    }
                },

                new Contact
                {
                    FirstName = "David",
                    LastName = "Taylor",
                    Street = "11 Station Road",
                    City = "Leeds",
                    PostalCode = "LS1 1BA",
                    Country = "United Kingdom",
                    Note = "Equipment supplier",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900105"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Business",
                            Value = "david.taylor@example.com"
                        }
                    },
                    Tags =
                    {
                        supplierTag
                    }
                },

                new Contact
                {
                    FirstName = "Jessica",
                    LastName = "Anderson",
                    Street = "75 Baker Street",
                    City = "London",
                    PostalCode = "W1U 6RT",
                    Country = "United Kingdom",
                    Note = "Friend from school",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900106"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "jessica.anderson@example.com"
                        }
                    },
                    Tags =
                    {
                        friendTag
                    }
                },

                new Contact
                {
                    FirstName = "Daniel",
                    LastName = "Thomas",
                    Street = "33 High Street",
                    City = "Bristol",
                    PostalCode = "BS1 2AW",
                    Country = "United Kingdom",
                    Note = "Development team member",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900107"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "daniel.thomas@example.com"
                        },
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "daniel.t@example.net"
                        }
                    },
                    Tags =
                    {
                        workTag,
                        friendTag
                    }
                },

                new Contact
                {
                    FirstName = "Olivia",
                    LastName = "Martin",
                    Street = "16 Church Lane",
                    City = "Cambridge",
                    PostalCode = "CB1 1NP",
                    Country = "United Kingdom",
                    Note = "Consulting client",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900108"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "olivia.martin@example.com"
                        }
                    },
                    Tags =
                    {
                        clientTag
                    }
                },

                new Contact
                {
                    FirstName = "James",
                    LastName = "Jackson",
                    Street = "9 Victoria Road",
                    City = "Oxford",
                    PostalCode = "OX1 1AA",
                    Country = "United Kingdom",
                    Note = "Family friend",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900109"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "james.jackson@example.com"
                        }
                    },
                    Tags =
                    {
                        friendTag,
                        familyTag
                    }
                },

                new Contact
                {
                    FirstName = "Sophia",
                    LastName = "White",
                    Street = "24 Castle Street",
                    City = "Edinburgh",
                    PostalCode = "EH1 2EU",
                    Country = "United Kingdom",
                    Note = "Design supplier",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900110"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Business",
                            Value = "sophia.white@example.com"
                        }
                    },
                    Tags =
                    {
                        supplierTag
                    }
                },

                new Contact
                {
                    FirstName = "Robert",
                    LastName = "Harris",
                    Street = "5 George Square",
                    City = "Glasgow",
                    PostalCode = "G2 1DY",
                    Country = "United Kingdom",
                    Note = "Project manager",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900111"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "robert.harris@example.com"
                        }
                    },
                    Tags =
                    {
                        workTag
                    }
                },

                new Contact
                {
                    FirstName = "Emma",
                    LastName = "Clark",
                    Street = "18 Market Street",
                    City = "York",
                    PostalCode = "YO1 8RA",
                    Country = "United Kingdom",
                    Note = "Close friend",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900112"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "emma.clark@example.com"
                        }
                    },
                    Tags =
                    {
                        friendTag
                    }
                },

                new Contact
                {
                    FirstName = "William",
                    LastName = "Lewis",
                    Street = "61 Bridge Road",
                    City = "Bath",
                    PostalCode = "BA1 1LN",
                    Country = "United Kingdom",
                    Note = "Business customer",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900113"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "william.lewis@example.com"
                        }
                    },
                    Tags =
                    {
                        clientTag
                    }
                },

                new Contact
                {
                    FirstName = "Charlotte",
                    LastName = "Walker",
                    Street = "32 Green Lane",
                    City = "Nottingham",
                    PostalCode = "NG1 2AS",
                    Country = "United Kingdom",
                    Note = "Sales department",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900114"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "charlotte.walker@example.com"
                        }
                    },
                    Tags =
                    {
                        workTag
                    }
                },

                new Contact
                {
                    FirstName = "Henry",
                    LastName = "Hall",
                    Street = "40 Mill Road",
                    City = "Sheffield",
                    PostalCode = "S1 2HH",
                    Country = "United Kingdom",
                    Note = "Office supplier",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900115"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Business",
                            Value = "henry.hall@example.com"
                        }
                    },
                    Tags =
                    {
                        supplierTag
                    }
                },

                new Contact
                {
                    FirstName = "Amelia",
                    LastName = "Young",
                    Street = "27 River Street",
                    City = "Newcastle",
                    PostalCode = "NE1 3DE",
                    Country = "United Kingdom",
                    Note = "Childhood friend",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900116"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "amelia.young@example.com"
                        }
                    },
                    Tags =
                    {
                        friendTag
                    }
                },

                new Contact
                {
                    FirstName = "Thomas",
                    LastName = "King",
                    Street = "12 College Road",
                    City = "Brighton",
                    PostalCode = "BN1 1AA",
                    Country = "United Kingdom",
                    Note = "Enterprise client",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900117"
                        },
                        new PhoneNumber
                        {
                            Type = "Office",
                            Value = "+44 1273 555 117"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "thomas.king@example.com"
                        }
                    },
                    Tags =
                    {
                        clientTag,
                        workTag
                    }
                },

                new Contact
                {
                    FirstName = "Grace",
                    LastName = "Wright",
                    Street = "89 North Road",
                    City = "Cardiff",
                    PostalCode = "CF10 1EP",
                    Country = "United Kingdom",
                    Note = "Family contact",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900118"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "grace.wright@example.com"
                        }
                    },
                    Tags =
                    {
                        familyTag
                    }
                },

                new Contact
                {
                    FirstName = "George",
                    LastName = "Scott",
                    Street = "19 Harbour Road",
                    City = "Plymouth",
                    PostalCode = "PL1 2AA",
                    Country = "United Kingdom",
                    Note = "Logistics supplier",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Work",
                            Value = "+44 7700 900119"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Business",
                            Value = "george.scott@example.com"
                        }
                    },
                    Tags =
                    {
                        supplierTag
                    }
                },

                new Contact
                {
                    FirstName = "Lily",
                    LastName = "Green",
                    Street = "55 Central Avenue",
                    City = "Leicester",
                    PostalCode = "LE1 5YP",
                    Country = "United Kingdom",
                    Note = "Friend and client",
                    PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = "Mobile",
                            Value = "+44 7700 900120"
                        }
                    },
                    EmailAddresses =
                    {
                        new EmailAddress
                        {
                            Type = "Personal",
                            Value = "lily.green@example.com"
                        },
                        new EmailAddress
                        {
                            Type = "Work",
                            Value = "lily.green@business.example"
                        }
                    },
                    Tags =
                    {
                        friendTag,
                        clientTag
                    }
                }
            };

            context.Contacts.AddRange(
                contacts
            );

            await context.SaveChangesAsync();
        }

        private static async Task<Tag> GetOrCreateTagAsync(
            AppDbContext context,
            string name
        )
        {
            var tag =
                await context.Tags
                    .FirstOrDefaultAsync(
                        t =>
                            t.Name.ToLower() ==
                            name.ToLower()
                    );

            if (tag != null)
            {
                return tag;
            }

            tag = new Tag
            {
                Name = name
            };

            context.Tags.Add(tag);

            return tag;
        }
    }
}