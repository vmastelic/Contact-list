using ContactManager.Api.DTOs.ContactDTOs;
using ContactManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(
            IContactService contactService
        )
        {
            _contactService =
                contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts =
                await _contactService.GetAllAsync();

            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id
        )
        {
            var contact =
                await _contactService
                    .GetByIdAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateContactDto dto
        )
        {
            var createdContact =
                await _contactService
                    .CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = createdContact.Id
                },
                createdContact
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateContactDto dto
        )
        {
            var updatedContact =
                await _contactService
                    .UpdateAsync(
                        id,
                        dto
                    );

            if (updatedContact == null)
            {
                return NotFound();
            }

            return Ok(updatedContact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id
        )
        {
            var deleted =
                await _contactService
                    .DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
            [FromQuery] int tagId
        )
        {
            var contacts =
                await _contactService
                    .FilterAsync(tagId);

            return Ok(contacts);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "lastName",
            [FromQuery] string sortDirection = "asc",
            [FromQuery] int? tagId = null
        )
        {
            var result =
                await _contactService
                    .SearchAsync(
                        search,
                        page,
                        pageSize,
                        sortBy,
                        sortDirection,
                        tagId
                    );

            return Ok(result);
        }
    }
}