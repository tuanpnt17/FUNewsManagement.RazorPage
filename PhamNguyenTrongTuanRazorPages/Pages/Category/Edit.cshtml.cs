using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhamNguyenTrongTuanRazorPages.Models.Category;
using ServiceLayer.Category;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Pages.Category
{
    [Authorize(Roles = "Staff")]
    public class EditModel(ICategoryService categoryService, IMapper mapper) : PageModel
    {
        [BindProperty]
        public UpdateCategoryViewModel Category { get; set; } = null!;

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
            Category = mapper.Map<UpdateCategoryViewModel>(categoryDto);

            var parent = await categoryService.GetParentCategoreisAsync();
            var parentCategories = mapper
                .Map<IEnumerable<ParentCategoryViewModel>>(parent)
                .ToList();

            // Insert the "None" option
            parentCategories.Insert(
                0,
                new ParentCategoryViewModel { CategoryId = 0, CategoryName = "None" }
            );

            ViewData["ParentCategoryId"] = new SelectList(
                parentCategories,
                "CategoryId",
                "CategoryName"
            );
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoryDto = mapper.Map<CategoryDTO>(Category);
            var result = await categoryService.UpdateCategoryAsync(categoryDto);
            if (result != null)
            {
                return RedirectToPage("./Index");
            }
            return BadRequest();
        }
    }
}
