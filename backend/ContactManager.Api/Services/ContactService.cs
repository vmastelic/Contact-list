using ContactManager.Api.Data;
using ContactManager.Api.DTOs;
using ContactManager.Api.DTOs.ContactDTOs;
using ContactManager.Api.DTOs.TagDTOs;
using ContactManager.Api.Entities;
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

        public async Task<ContactDto?> UpdateAsync(int id, UpdateContactDto dto)
        {

            var contact = await _context.Contacts
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses)
                .Include(c => c.Tags)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return null;
            }

            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.Street = dto.Street;
            contact.City = dto.City;
            contact.PostalCode = dto.PostalCode;
            contact.Country = dto.Country;
            contact.Note = dto.Note;

            _context.PhoneNumbers.RemoveRange(contact.PhoneNumbers);
            _context.EmailAddresses.RemoveRange(contact.EmailAddresses);

            contact.PhoneNumbers.Clear();
            contact.EmailAddresses.Clear();
            contact.Tags.Clear();

            foreach(var phone in dto.PhoneNumbers)
            {
                contact.PhoneNumbers.Add(new PhoneNumber
                {
                    Type = phone.Type,
                    Value = phone.Value
                });
            }

            foreach(var email in dto.EmailAddresses)
            {
                contact.EmailAddresses.Add(new EmailAddress
                {
                    Type = email.Type,
                    Value = email.Value
                });
            }

            var tags = await _context.Tags
                .Where(t => dto.TagIds.Contains(t.Id))
                .ToListAsync();

            foreach (var tag in tags)
            {
                contact.Tags.Add(tag);
            }

            await _context.SaveChangesAsync();

            return await GetByIdAsync(contact.Id);
        }

        public async Task<bool>  DeleteAsync(int id)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return false;
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<ContactDto>> FilterAsync(int tagId)
        {
            return await _context.Contacts
                .AsNoTracking()
                .Where(c => c.Tags.Any(t => t.Id == tagId))
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
    }
}