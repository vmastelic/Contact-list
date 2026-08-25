using System.ComponentModel.DataAnnotations;

namespace ContactManager.Api.DTOs
{
    public class EmailAddressDto
    {
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Value { get; set; } = string.Empty;
    }
}