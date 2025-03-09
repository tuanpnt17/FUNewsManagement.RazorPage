using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    public class CreateModel : PageModel
    {
        private readonly Repository.Data.FuNewsDbContext _context;

        public CreateModel(Repository.Data.FuNewsDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(
                _context.Categories,
                "CategoryId",
                "CategoryDesciption"
            );
            ViewData["CreatedById"] = new SelectList(
                _context.SystemAccounts,
                "AccountId",
                "AccountId"
            );
            return Page();
        }

        [BindProperty]
        public Repository.Entities.NewsArticle NewsArticle { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.NewsArticles.Add(NewsArticle);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
