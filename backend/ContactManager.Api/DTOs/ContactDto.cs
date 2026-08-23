using ContactManager.Api.Entities;

namespace ContactManager.Api.DTOs
{
    public class ContactDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
            = new List<PhoneNumber>();
        public ICollection<EmailAddress> EmailAddresses { get; set; }
            = new List<EmailAddress>();
        public ICollection<Tag> Tags { get; set; }
            = new List<Tag>();
    }
}
