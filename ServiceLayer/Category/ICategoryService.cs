using Repository.Data;
using ServiceLayer.Models;

namespace ServiceLayer.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId);
        Task<IEnumerable<CategoryDTO>> GetParentCategoreisAsync();

        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto);

        Task<int?> UpdateCategoryAsync(CategoryDTO categoryDto);
        Task<int?> DeleteCategoryAsync(int categoryId);
        Task<PaginatedList<Repository.Entities.Category>> ListCategoryWithPaginationAndFitler(
            string? searchString,
            string? sortOrder,
            int? pageNumber,
            int? pageSize
        );
    }
}
