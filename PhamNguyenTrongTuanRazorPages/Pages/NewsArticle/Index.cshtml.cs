using Microsoft.EntityFrameworkCore;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    public class IndexModel(FuNewsDbContext context) : PageModel
    {
        public IList<Repository.Entities.NewsArticle> NewsArticle { get; set; } = default!;

        public async Task OnGetAsync()
        {
            NewsArticle = await context
                .NewsArticles.Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .ToListAsync();
        }
    }
}
