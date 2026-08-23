using ContactManager.Api.DTOs.TagDTOs;

namespace ContactManager.Api.Services
{
    public interface ITagService
    {
        Task<TagDto?> CreateAsync(CreateTagDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
