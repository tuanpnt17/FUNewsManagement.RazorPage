using PhamNguyenTrongTuanRazorPages.Models.NewsArticle;
using ServiceLayer.NewsArticle;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    [Authorize(Roles = "Staff")]
    public class IndexModel(INewsArticleService newsArticleService, IMapper mapper) : PageModel
    {
        public IList<ViewNewsArticleViewModel> NewsArticle { get; set; } = null!;

        public async Task OnGetAsync()
        {
            var articleDtos = await newsArticleService.GetAllNewsArticleAsync();
            var articlesVm = mapper.Map<IList<ViewNewsArticleViewModel>>(articleDtos);

            NewsArticle = articlesVm;
        }
    }
}
