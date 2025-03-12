using Microsoft.AspNetCore.Authorization;
using PhamNguyenTrongTuanRazorPages.Models.Category;
using ServiceLayer.Category;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Pages.Category
{
    [Authorize(Roles = "Staff")]
    public class IndexModel(ICategoryService categoryService, IMapper mapper) : PageModel
    {
        public PaginatedList<CategoryViewModel> Category { get; set; } = null!;

        public async Task OnGetAsync(
            string sortOrder,
            string currentFilter,
            string? searchString,
            int? pageNumber,
            int? pageSize
        )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["IdSortParam"] = sortOrder == "id" ? "" : "id";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var categories = await categoryService.ListCategoryWithPaginationAndFitler(
                searchString,
                sortOrder,
                pageNumber,
                pageSize
            );

            var categoriesDto = mapper.Map<PaginatedList<CategoryDTO>>(categories);
            var categoryViewModels = mapper.Map<PaginatedList<CategoryViewModel>>(categoriesDto);
            Category = new PaginatedList<CategoryViewModel>(
                categoryViewModels,
                categories.TotalPages,
                categories.TotalElements,
                categories.PageIndex
            );
        }
    }
}
