namespace ContactManager.Api.DTOs.ContactDTOs
{
    public class UpdateContactDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public List<PhoneNumberDto> PhoneNumbers { get; set; } = new();
        public List<EmailAddressDto> EmailAddresses { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
    }
}