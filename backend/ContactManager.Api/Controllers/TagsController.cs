using ContactManager.Api.DTOs;
using ContactManager.Api.DTOs.TagDTOs;
using ContactManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllAsync();

            return Ok(tags);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateTagDto dto
        )
        {
            var createdTag =
                await _tagService.CreateAsync(dto);

            if (createdTag == null)
            {
                return Conflict(new
                {
                    message = "A tag with this name already exists."
                });
            }

            return Ok(createdTag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id
        )
        {
            var deleted =
                await _tagService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}