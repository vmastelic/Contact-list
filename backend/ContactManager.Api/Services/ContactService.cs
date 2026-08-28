using ContactManager.Api.Data;
using ContactManager.Api.DTOs;
using ContactManager.Api.DTOs.ContactDTOs;
using ContactManager.Api.DTOs.TagDTOs;
using ContactManager.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContactManager.Api.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(
            AppDbContext context
        )
        {
            _context = context;
        }

        public async Task<List<ContactDto>> GetAllAsync()
        {
            return await _context.Contacts
                .AsNoTracking()
                .Select(ContactSelector())
                .ToListAsync();
        }

        public async Task<ContactDto?> GetByIdAsync(
            int id
        )
        {
            return await _context.Contacts
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(ContactSelector())
                .FirstOrDefaultAsync();
        }

        public async Task<ContactDto> CreateAsync(
            CreateContactDto dto
        )
        {
            var contact = new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Street = dto.Street,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Note = dto.Note
            };

            foreach (var phone in dto.PhoneNumbers)
            {
                contact.PhoneNumbers.Add(
                    new PhoneNumber
                    {
                        Type = phone.Type,
                        Value = phone.Value
                    }
                );
            }

            foreach (var email in dto.EmailAddresses)
            {
                contact.EmailAddresses.Add(
                    new EmailAddress
                    {
                        Type = email.Type,
                        Value = email.Value
                    }
                );
            }

            var tagIds =
                dto.TagIds
                    .Distinct()
                    .ToList();

            var tags =
                await _context.Tags
                    .Where(t =>
                        tagIds.Contains(t.Id)
                    )
                    .ToListAsync();

            foreach (var tag in tags)
            {
                contact.Tags.Add(tag);
            }

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            return await GetByIdAsync(contact.Id)
                ?? throw new InvalidOperationException();
        }

        public async Task<ContactDto?> UpdateAsync(
            int id,
            UpdateContactDto dto
        )
        {
            var contact =
                await _context.Contacts
                    .Include(c => c.PhoneNumbers)
                    .Include(c => c.EmailAddresses)
                    .Include(c => c.Tags)
                    .FirstOrDefaultAsync(
                        c => c.Id == id
                    );

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

            _context.PhoneNumbers.RemoveRange(
                contact.PhoneNumbers
            );

            _context.EmailAddresses.RemoveRange(
                contact.EmailAddresses
            );

            contact.PhoneNumbers.Clear();

            contact.EmailAddresses.Clear();

            foreach (var phone in dto.PhoneNumbers)
            {
                contact.PhoneNumbers.Add(
                    new PhoneNumber
                    {
                        Type = phone.Type,
                        Value = phone.Value
                    }
                );
            }

            foreach (var email in dto.EmailAddresses)
            {
                contact.EmailAddresses.Add(
                    new EmailAddress
                    {
                        Type = email.Type,
                        Value = email.Value
                    }
                );
            }

            contact.Tags.Clear();

            var tagIds =
                dto.TagIds
                    .Distinct()
                    .ToList();

            var tags =
                await _context.Tags
                    .Where(t =>
                        tagIds.Contains(t.Id)
                    )
                    .ToListAsync();

            foreach (var tag in tags)
            {
                contact.Tags.Add(tag);
            }

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(
            int id
        )
        {
            var contact =
                await _context.Contacts
                    .FirstOrDefaultAsync(
                        c => c.Id == id
                    );

            if (contact == null)
            {
                return false;
            }

            _context.Contacts.Remove(contact);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ContactDto>> FilterAsync(
            int tagId
        )
        {
            return await _context.Contacts
                .AsNoTracking()
                .Where(c =>
                    c.Tags.Any(
                        t => t.Id == tagId
                    )
                )
                .Select(ContactSelector())
                .ToListAsync();
        }

        public async Task<PagedResultDto<ContactDto>> SearchAsync(
            string? search,
            int page,
            int pageSize,
            string sortBy,
            string sortDirection,
            int? tagId
        )
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var query =
                _context.Contacts
                    .AsNoTracking()
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var text = search.Trim();

                query = query.Where(c =>
                    EF.Functions.ILike(
                        c.FirstName,
                        $"%{text}%"
                    )
                    ||
                    EF.Functions.ILike(
                        c.LastName,
                        $"%{text}%"
                    )
                    ||
                    c.PhoneNumbers.Any(p =>
                        EF.Functions.ILike(
                            p.Value,
                            $"%{text}%"
                        )
                    )
                    ||
                    c.EmailAddresses.Any(e =>
                        EF.Functions.ILike(
                            e.Value,
                            $"%{text}%"
                        )
                    )
                    ||
                    c.Tags.Any(t =>
                        EF.Functions.ILike(
                            t.Name,
                            $"%{text}%"
                        )
                    )
                );
            }

            if (tagId.HasValue)
            {
                query = query.Where(c =>
                    c.Tags.Any(
                        t => t.Id == tagId.Value
                    )
                );
            }

            var totalCount =
                await query.CountAsync();

            var descending =
                sortDirection.Equals(
                    "desc",
                    StringComparison.OrdinalIgnoreCase
                );

            query =
                sortBy.ToLower() switch
                {
                    "firstname" =>
                        descending
                            ? query.OrderByDescending(
                                c => c.FirstName
                            )
                            : query.OrderBy(
                                c => c.FirstName
                            ),

                    _ =>
                        descending
                            ? query.OrderByDescending(
                                c => c.LastName
                            )
                            : query.OrderBy(
                                c => c.LastName
                            )
                };

            var items =
                await query
                    .Skip(
                        (page - 1) * pageSize
                    )
                    .Take(pageSize)
                    .Select(ContactSelector())
                    .ToListAsync();

            var totalPages =
                totalCount == 0
                    ? 0
                    : (int)Math.Ceiling(
                        totalCount /
                        (double)pageSize
                    );

            return new PagedResultDto<ContactDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        private static Expression<Func<Contact, ContactDto>>
            ContactSelector()
        {
            return contact =>
                new ContactDto
                {
                    Id = contact.Id,

                    FirstName =
                        contact.FirstName,

                    LastName =
                        contact.LastName,

                    Street =
                        contact.Street,

                    City =
                        contact.City,

                    PostalCode =
                        contact.PostalCode,

                    Country =
                        contact.Country,

                    Note =
                        contact.Note,

                    PhoneNumbers =
                        contact.PhoneNumbers
                            .Select(phone =>
                                new PhoneNumberDto
                                {
                                    Type =
                                        phone.Type,

                                    Value =
                                        phone.Value
                                }
                            )
                            .ToList(),

                    EmailAddresses =
                        contact.EmailAddresses
                            .Select(email =>
                                new EmailAddressDto
                                {
                                    Type =
                                        email.Type,

                                    Value =
                                        email.Value
                                }
                            )
                            .ToList(),

                    Tags =
                        contact.Tags
                            .Select(tag =>
                                new TagDto
                                {
                                    Id =
                                        tag.Id,

                                    Name =
                                        tag.Name
                                }
                            )
                            .ToList()
                };
        }
    }
}