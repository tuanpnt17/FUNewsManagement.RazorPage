using ServiceLayer.Models;
using ServiceLayer.NewsArticle;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    public class DetailsModel(INewsArticleService newsArticleService) : PageModel
    {
        public NewsArticleDTO NewsArticle { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsarticle = await newsArticleService.GetActiveNewsArticleByIdAsync(id);
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
    }
}
