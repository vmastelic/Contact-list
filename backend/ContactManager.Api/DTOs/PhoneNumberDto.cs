using System.ComponentModel.DataAnnotations;

namespace ContactManager.Api.DTOs
{
    public class PhoneNumberDto
    {
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string Value { get; set; } = string.Empty;

    }
}
