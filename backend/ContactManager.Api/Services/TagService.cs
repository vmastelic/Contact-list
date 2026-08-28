using ContactManager.Api.Data;
using ContactManager.Api.DTOs.TagDTOs;
using ContactManager.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Api.Services
{
    public class TagService : ITagService
    {

        private readonly AppDbContext _context;

        public async Task<List<TagDto>> GetAllAsync()
        {
            return await _context.Tags
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .Select(t => new TagDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TagDto?> CreateAsync(CreateTagDto dto)
        {
            var tag = new Tag
            {
                Name = dto.Name,
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
