using PhamNguyenTrongTuanRazorPages.Models.NewsArticle;
using ServiceLayer.Account;
using ServiceLayer.Models;
using ServiceLayer.NewsArticle;

namespace PhamNguyenTrongTuanRazorPages.Pages.NewsArticle
{
    [Authorize(Roles = "Staff")]
    public class IndexModel(
        INewsArticleService newsArticleService,
        IMapper mapper,
        IAccountService accountService
    ) : PageModel
    {
        public PaginatedList<ViewNewsArticleViewModel> NewsArticle { get; set; } = null!;

        public async Task OnGetAsync(
            string sortOrder,
            string currentFilter,
            string? searchString,
            int? pageNumber,
            int? pageSize
        )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParam"] = sortOrder == "title" ? "title_desc" : "title";
            ViewData["DateSortParam"] = sortOrder == "date" ? "" : "date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var articles = await newsArticleService.ListArticlesWithPaginationAndFitler(
                searchString,
                sortOrder,
                pageNumber,
                pageSize
            );
            var articleDtos = mapper.Map<PaginatedList<NewsArticleDTO>>(articles);
            var allNewsArticleAsync = articleDtos.ToList();
            foreach (var article in allNewsArticleAsync)
            {
                var updatedBy = await accountService.GetAcountByIdAsync(article.UpdatedById);
                if (updatedBy != null)
                {
                    article.UpdatedBy = updatedBy;
                }
            }
            var newsArticleViewModels = mapper.Map<PaginatedList<ViewNewsArticleViewModel>>(
                articleDtos
            );
            NewsArticle = new PaginatedList<ViewNewsArticleViewModel>(
                newsArticleViewModels,
                articles.TotalPages,
                articles.TotalElements,
                articles.PageIndex
            );
        }

        public async Task<IActionResult> OnGetAllNewsArticlesAsync(
            string sortOrder,
            string? searchString,
            int? pageNumber,
            int? pageSize
        )
        {
            var articles = await newsArticleService.ListArticlesWithPaginationAndFitler(
                searchString,
                sortOrder,
                pageNumber,
                pageSize
            );
            var articleDtos = mapper.Map<PaginatedList<NewsArticleDTO>>(articles);
            var allNewsArticleAsync = articleDtos.ToList();
            foreach (var article in allNewsArticleAsync)
            {
                var updatedBy = await accountService.GetAcountByIdAsync(article.UpdatedById);
                if (updatedBy != null)
                {
                    article.UpdatedBy = updatedBy;
                }
            }
            var newsArticleViewModels = mapper.Map<PaginatedList<ViewNewsArticleViewModel>>(
                articleDtos
            );
            var result = new
            {
                newsArticles = newsArticleViewModels,
                totalPages = articles.TotalPages,
                totalElements = articles.TotalElements,
                pageIndex = articles.PageIndex,
                hasPreviousPage = articles.PageIndex > 1,
                hasNextPage = articles.PageIndex < articles.TotalPages,
                currentSort = sortOrder,
                titleSortParam = sortOrder == "title" ? "title_desc" : "title",
                dateSortParam = sortOrder == "date" ? "" : "date",
                searchString,
            };
            return new JsonResult(result);
        }
    }
}
