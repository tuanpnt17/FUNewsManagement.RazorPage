using PhamNguyenTrongTuanRazorPages.Models.NewsArticle;
using ServiceLayer.NewsArticle;

namespace PhamNguyenTrongTuanRazorPages.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class IndexModel(INewsArticleService newsArticleService, IMapper mapper) : PageModel
    {
        public IEnumerable<ViewNewsArticleViewModel> ViewNewsArticles { get; set; }

        public async Task OnGet(
            DateTime? startDate,
            DateTime? endDate,
            int? pageNumber,
            int? pageSize
        )
        {
            startDate ??= DateTime.UtcNow.AddDays(-30);
            endDate ??= DateTime.UtcNow;
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;
            var newsArticles = await newsArticleService.GetAllNewsArticleAsync();
            if (startDate <= endDate)
            {
                newsArticles = newsArticles.Where(n =>
                    n.ModifiedDate.Date >= startDate.Value.Date
                    && n.ModifiedDate.Date <= endDate.Value.Date
                );
            }
            else
            {
                TempData["DateRangeError"] =
                    "Start date must be earlier than end date. Please adjust the dates.";
            }

            ViewNewsArticles = mapper.Map<IEnumerable<ViewNewsArticleViewModel>>(newsArticles);
        }
    }
}
