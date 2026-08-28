using ContactManager.Api.DTOs;
using ContactManager.Api.DTOs.ContactDTOs;

namespace ContactManager.Api.Services
{
    public interface IContactService
    {
        Task<List<ContactDto>> GetAllAsync();

        Task<ContactDto?> GetByIdAsync(int id);

        Task<ContactDto> CreateAsync(CreateContactDto dto);

        Task<ContactDto?> UpdateAsync(
            int id,
            UpdateContactDto dto
        );

        Task<bool> DeleteAsync(int id);

        Task<List<ContactDto>> FilterAsync(
            int tagId
        );

        Task<PagedResultDto<ContactDto>> SearchAsync(
            string? search,
            int page,
            int pageSize,
            string sortBy,
            string sortDirection,
            int? tagId
        );
    }
}