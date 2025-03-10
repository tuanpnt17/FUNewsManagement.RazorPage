using PhamNguyenTrongTuanRazorPages.Models.Category;
using ServiceLayer.Category;

namespace PhamNguyenTrongTuanRazorPages.Pages.Category
{
    public class IndexModel(ICategoryService categoryService, IMapper mapper) : PageModel
    {
        public IList<CategoryViewModel> Category { get; set; } = null!;

        public async Task OnGetAsync()
        {
            var categoryDtos = await categoryService.GetCategoriesAsync();

            Category =
                (IList<CategoryViewModel>)mapper.Map<IEnumerable<CategoryViewModel>>(categoryDtos);
        }
    }
}
