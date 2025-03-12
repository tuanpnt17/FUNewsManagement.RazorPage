using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;

namespace Repository.Tags;

public class TagRepository : ITagRepository
{
    private readonly FuNewsDbContext _context;

    public TagRepository(FuNewsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tag>> ListAllAsync()
    {
        var tags = await _context.Tags.ToListAsync();
        return tags;
    }

    public Task<Tag> CreateAsync(Tag t)
    {
        throw new NotImplementedException();
    }

    public Task<int?> UpdateAsync(Tag t)
    {
        throw new NotImplementedException();
    }

    public Task<int?> DeleteAsync(Tag? t)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Tag>> GetTagsByIdsAsync(IEnumerable<int> articleDtoTagIds)
    {
        var tags = await _context.Tags.Where(t => articleDtoTagIds.Contains(t.TagId)).ToListAsync();
        return tags;
    }
}
