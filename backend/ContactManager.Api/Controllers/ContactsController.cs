using ContactManager.Api.DTOs;
using ContactManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _contactService.GetAllAsync();
            return Ok(contacts);
        }

        [HttpGet(" {id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContactDto dto)
        {
            var contact = await _contactService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = contact.Id },
                contact
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateContactDto dto)
        {
            var contact = await _contactService.UpdateAsync(id, dto);

            if (contact == null)
            {
                return NotFound();
            }
            
            return Ok(contact);
        }
    }
}
