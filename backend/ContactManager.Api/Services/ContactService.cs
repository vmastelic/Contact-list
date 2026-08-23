using ContactManager.Api.Data;
using ContactManager.Api.DTOs;
using ContactManager.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

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
        
        public async Task<ContactDto> CreateAsync(CreateContactDto dto)
        {
            var tags = await _context.Tags
                .Where(t => dto.TagIds.Contains(t.Id))
                .ToListAsync();

            var contact = new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Street = dto.Street,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Note = dto.Note,

                PhoneNumbers = dto.PhoneNumbers
                .Select(p => new PhoneNumber
                {
                    Type = p.Type,
                    Value = p.Value
                })
                .ToList(),

                EmailAddresses = dto.EmailAddresses
                .Select(e => new EmailAddress
                {
                    Type = e.Type,
                    Value = e.Value
                })
                .ToList(),

                Tags = tags
            };

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            return new ContactDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Street = contact.Street,
                City = contact.City,
                PostalCode = contact.PostalCode,
                Country = contact.Country,
                Note = contact.Note
            };
        }
    }
}