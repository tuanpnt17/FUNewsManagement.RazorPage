using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Repository.Data.FuNewsDbContext _context;

        public EditModel(Repository.Data.FuNewsDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Repository.Entities.NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsarticle = await _context.NewsArticles.FirstOrDefaultAsync(m =>
                m.NewsArticleId == id
            );
            if (newsarticle == null)
            {
                return NotFound();
            }
            NewsArticle = newsarticle;
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(NewsArticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsArticleExists(NewsArticle.NewsArticleId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool NewsArticleExists(string id)
        {
            return _context.NewsArticles.Any(e => e.NewsArticleId == id);
        }
    }
}
