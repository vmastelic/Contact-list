using ContactManager.Api.DTOs.ContactDTOs;
using ContactManager.Api.DTOs.TagDTOs;

namespace ContactManager.Api.Services
{
    public interface IContactService
    {
        Task<List<ContactDto>> GetAllAsync();
        Task<ContactDto> GetByIdAsync(int id);
        Task<ContactDto> CreateAsync(CreateContactDto dto);
        Task<ContactDto> UpdateAsync(int id, UpdateContactDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<ContactDto>> FilterAsync(int tagId);
        
    }
}
