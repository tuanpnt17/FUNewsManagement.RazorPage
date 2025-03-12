using AutoMapper;
using Repository.Categories;
using Repository.Data;
using ServiceLayer.Models;

namespace ServiceLayer.Category
{
    public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        : ICategoryService
    {
        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await categoryRepository.ListAllAsync();
            var categoryDtos = mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return categoryDtos;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            var categoryDto = mapper.Map<CategoryDTO>(category);
            return categoryDto;
        }

        public async Task<IEnumerable<CategoryDTO>> GetParentCategoreisAsync()
        {
            var category = await categoryRepository.ListAllAsync();
            var parent = category.Where(c => c.ParentCategoryId == null);
            return mapper.Map<IEnumerable<CategoryDTO>>(parent);
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto)
        {
            var category = mapper.Map<Repository.Entities.Category>(categoryDto);
            var addedCategory = await categoryRepository.CreateAsync(category);
            var categoryDtoToReturn = mapper.Map<CategoryDTO>(addedCategory);
            return categoryDtoToReturn;
        }

        public async Task<int?> UpdateCategoryAsync(CategoryDTO categoryDto)
        {
            var category = mapper.Map<Repository.Entities.Category>(categoryDto);
            var effectedRow = await categoryRepository.UpdateAsync(category);
            return effectedRow;
        }

        public async Task<int?> DeleteCategoryAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
                return 0;
            var countNews = category.NewsArticles?.Count();
            if (countNews == null || countNews > 0)
            {
                return 0;
            }
            var effectedRow = await categoryRepository.DeleteAsync(category);
            return effectedRow;
        }

        public async Task<
            PaginatedList<Repository.Entities.Category>
        > ListCategoryWithPaginationAndFitler(
            string? searchString,
            string? sortOrder,
            int? pageNumber,
            int? pageSize
        )
        {
            pageNumber ??= 1;
            pageSize ??= 8;
            return await categoryRepository.GetCategoriesQuery(
                (int)pageNumber,
                (int)pageSize,
                searchString,
                sortOrder
            );
        }
    }
}
