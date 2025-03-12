using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhamNguyenTrongTuanRazorPages.Models.Category;
using ServiceLayer.Category;
using ServiceLayer.Models;

namespace PhamNguyenTrongTuanRazorPages.Pages.Category
{
    [Authorize(Roles = "Staff")]
    public class CreateModel(ICategoryService categoryService, IMapper mapper) : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var parent = await categoryService.GetParentCategoreisAsync();
            var parentCategories = mapper
                .Map<IEnumerable<ParentCategoryViewModel>>(parent)
                .ToList();

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

        [BindProperty]
        public AddNewCategoryViewModel Category { get; set; } = null!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoryDto = mapper.Map<CategoryDTO>(Category);
            await categoryService.CreateCategoryAsync(categoryDto);
            return RedirectToPage("./Index");
        }
    }
}
