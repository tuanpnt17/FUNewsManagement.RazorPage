namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly Repository.Data.FuNewsDbContext _context;

        public DeleteModel(Repository.Data.FuNewsDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Repository.Entities.NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
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
            else
            {
                NewsArticle = newsarticle;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsarticle = await _context.NewsArticles.FindAsync(id);
            if (newsarticle != null)
            {
                NewsArticle = newsarticle;
                _context.NewsArticles.Remove(NewsArticle);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
