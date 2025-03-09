using Repository.Data;
using Repository.Entities;

namespace Repository.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByIdAsync(int id);
    }
}
