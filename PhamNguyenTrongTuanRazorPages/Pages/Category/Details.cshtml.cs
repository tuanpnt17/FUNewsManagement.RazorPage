using Microsoft.AspNetCore.Authorization;
using PhamNguyenTrongTuanRazorPages.Models.Category;
using ServiceLayer.Category;

namespace PhamNguyenTrongTuanRazorPages.Pages.Category
{
    [Authorize(Roles = "Staff")]
    public class DetailsModel(ICategoryService categoryService, IMapper mapper) : PageModel
    {
        public CategoryViewModel Category { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDto = await categoryService.GetCategoryByIdAsync((int)id);

            if (categoryDto == null)
            {
                return NotFound();
            }
            else
            {
                Category = mapper.Map<CategoryViewModel>(categoryDto);
            }
            return Page();
        }
    }
}
