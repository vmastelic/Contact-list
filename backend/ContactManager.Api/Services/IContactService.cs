using ContactManager.Api.DTOs;

namespace ContactManager.Api.Services
{
    public interface IContactService
    {
        Task<List<ContactDto>> GetAllAsync();
        Task<ContactDto> GetByIdAsync(int id);
        Task<ContactDto> CreateAsync(CreateContactDto dto);
    }
}
