using PhamNguyenTrongTuanRazorPages.Models.NewsArticle;
using ServiceLayer.NewsArticle;

namespace PhamNguyenTrongTuanRazorPages.Pages
{
    public class IndexModel(INewsArticleService newsArticleService, IMapper mapper) : PageModel
    {
        public IEnumerable<NewsArticleViewModel> NewsArticles { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var articleDtos = await newsArticleService.GetActiveNewsArticlesAsync();
            NewsArticles = mapper.Map<IEnumerable<NewsArticleViewModel>>(articleDtos);
        }
    }
}
