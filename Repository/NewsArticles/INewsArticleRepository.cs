using Repository.Data;
using Repository.Entities;

namespace Repository.NewsArticles
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<NewsArticle?> GetArticleByIdAsync(string id);
        Task<PaginatedList<NewsArticle>> GetCategoriesQuery(
            int pageNumber,
            int pageSize,
            string? searchString,
            string? sortOrder
        );
    }
}
