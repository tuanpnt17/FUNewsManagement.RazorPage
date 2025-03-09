using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Data;

namespace PhamNguyenTrongTuanRazorPages.Pages;

public class IndexModel(ILogger<IndexModel> logger, FuNewsDbContext context) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    public string? ArticleName { get; set; }

    public void OnGet(string articleId = "1")
    {
        ArticleName = context
            .NewsArticles.FirstOrDefault(a => a.NewsArticleId == articleId)
            ?.NewsTitle;
    }
}
