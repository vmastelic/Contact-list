using System.ComponentModel.DataAnnotations;

namespace ContactManager.Api.DTOs.TagDTOs
{
    public class CreateTagDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
