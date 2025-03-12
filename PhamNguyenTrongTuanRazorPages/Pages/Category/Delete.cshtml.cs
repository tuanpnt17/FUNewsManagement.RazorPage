using Microsoft.AspNetCore.Authorization;
using PhamNguyenTrongTuanRazorPages.Models.Category;
using ServiceLayer.Category;

namespace PhamNguyenTrongTuanRazorPages.Pages.Category
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel(ICategoryService categoryService, IMapper mapper) : PageModel
    {
        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleteEffected = await categoryService.DeleteCategoryAsync((int)id);
            if (deleteEffected > 0)
            {
                return RedirectToPage("./Index");
            }
            ModelState.AddModelError(
                "DeleteFailed",
                "This category contains news article. Delete failed!!!"
            );
            var categoryDto = await categoryService.GetCategoryByIdAsync((int)id);
            Category = mapper.Map<CategoryViewModel>(categoryDto);
            return Page();
        }
    }
}
