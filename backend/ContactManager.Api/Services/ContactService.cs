using ContactManager.Api.Data;
using ContactManager.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Api.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContactDto>> GetAllAsync()
        {
            return await _context.Contacts
                .AsNoTracking()
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses)
                .Include(c => c.Tags)
                .Select(c => new ContactDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Street = c.Street,
                    City = c.City,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Note = c.Note,

                    PhoneNumbers = c.PhoneNumbers
                        .Select(p => new PhoneNumberDto
                        {
                            Type = p.Type,
                            Value = p.Value
                        })
                        .ToList(),

                    EmailAddresses = c.EmailAddresses
                        .Select(e => new EmailAddressDto
                        {
                            Type = e.Type,
                            Value = e.Value
                        })
                        .ToList(),

                    Tags = c.Tags
                        .Select(t => new TagDto
                        {
                            Id = t.Id,
                            Name = t.Name
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<ContactDto?> GetByIdAsync(int id)
        {
            return await _context.Contacts
                .AsNoTracking()
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses)
                .Include(c => c.Tags)
                .Where(c => c.Id == id)
                .Select(c => new ContactDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Street = c.Street,
                    City = c.City,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Note = c.Note,

                    PhoneNumbers = c.PhoneNumbers
                        .Select(p => new PhoneNumberDto
                        {
                            Type = p.Type,
                            Value = p.Value
                        })
                        .ToList(),

                    EmailAddresses = c.EmailAddresses
                        .Select(e => new EmailAddressDto
                        {
                            Type = e.Type,
                            Value = e.Value
                        })
                        .ToList(),

                    Tags = c.Tags
                        .Select(t => new TagDto
                        {
                            Id = t.Id,
                            Name = t.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}