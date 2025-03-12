using Repository.Data;
using Repository.Entities;

namespace Repository.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<PaginatedList<Category>> GetCategoriesQuery(
            int pageNumber,
            int pageSize,
            string? searchString,
            string? sortOrder
        );
    }
}
