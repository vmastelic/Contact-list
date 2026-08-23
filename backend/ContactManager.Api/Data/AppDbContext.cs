using ContactManager.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<PhoneNumber> PhoneNumbers { get; set; } = null!;
        public DbSet<EmailAddress> EmailAddresses { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
    }
}
