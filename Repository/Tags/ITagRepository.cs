using Repository.Data;
using Repository.Entities;

namespace Repository.Tags;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<ICollection<Tag>> GetTagsByIdsAsync(IEnumerable<int> articleDtoTagIds);
}
