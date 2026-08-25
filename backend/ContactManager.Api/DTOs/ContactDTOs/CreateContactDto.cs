using System.ComponentModel.DataAnnotations;

namespace ContactManager.Api.DTOs.ContactDTOs
{
    public class CreateContactDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(100)]
        public string PostalCode { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Note { get; set; } = string.Empty;

        public List<PhoneNumberDto> PhoneNumbers { get; set; } = new();
        public List<EmailAddressDto> EmailAddresses { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
    }
}