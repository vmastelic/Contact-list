using ContactManager.Api.DTOs.TagDTOs;

namespace ContactManager.Api.DTOs.ContactDTOs
{
    public class ContactDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public List<PhoneNumberDto> PhoneNumbers { get; set; } = new();
        public List<EmailAddressDto> EmailAddresses { get; set; } = new();
        public List<TagDto> Tags { get; set; } = new();
    }
}