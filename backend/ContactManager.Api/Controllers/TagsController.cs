using ContactManager.Api.DTOs.TagDTOs;
using ContactManager.Api.Entities;
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagDto dto)
        {
            var tag = await _tagService.CreateAsync(dto);
            return Ok(tag);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tagService.DeleteAsync(id);
            if (!deleted) return NotFound(); ;
            return NoContent();
        }
    }
}
