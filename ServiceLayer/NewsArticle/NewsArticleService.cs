using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Repository.Data;
using Repository.NewsArticles;
using ServiceLayer.Account;
using ServiceLayer.Models;
using ServiceLayer.Tag;

namespace ServiceLayer.NewsArticle
{
    public class NewsArticleService(
        INewsArticleRepository articleRepository,
        IMapper mapper,
        ITagService tagService,
        IAccountService accountService
    ) : INewsArticleService
    {
        public async Task<IEnumerable<NewsArticleDTO>> GetActiveNewsArticlesAsync()
        {
            var articles = await articleRepository.ListAllAsync();
            articles = articles.Where(a => a.NewsStatus == true);
            var articleDtos = mapper.Map<IEnumerable<NewsArticleDTO>>(articles);
            return articleDtos;
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllNewsArticleAsync()
        {
            var articles = await articleRepository.ListAllAsync();
            articles = articles.OrderByDescending(a => a.ModifiedDate);
            var articleDtos = mapper.Map<IEnumerable<NewsArticleDTO>>(articles);
            var allNewsArticleAsync = articleDtos.ToList();
            foreach (var article in allNewsArticleAsync)
            {
                var updatedBy = await accountService.GetAcountByIdAsync(article.UpdatedById);
                if (updatedBy != null)
                {
                    article.UpdatedBy = updatedBy;
                }
            }
            return allNewsArticleAsync;
        }

        public async Task<NewsArticleDTO?> GetActiveNewsArticleByIdAsync(string articleId)
        {
            var article = await articleRepository.GetArticleByIdAsync(articleId);
            if (article == null || article.NewsStatus == false)
            {
                return null;
            }
            return mapper.Map<NewsArticleDTO>(article);
        }

        public async Task<NewsArticleDTO?> GetNewsArticleByIdAsync(string articleId)
        {
            var article = await articleRepository.GetArticleByIdAsync(articleId);
            if (article == null)
            {
                return null;
            }
            return mapper.Map<NewsArticleDTO>(article);
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetActiveTopRecentNewsArticlesAysnc(
            int count
        )
        {
            var articles = await articleRepository.ListAllAsync();
            articles = articles
                .Where(a => a.NewsStatus == true)
                .Take(count)
                .OrderByDescending(a => a.ModifiedDate);
            return mapper.Map<IEnumerable<NewsArticleDTO>>(articles);
        }

        public async Task<NewsArticleDTO> CreateNewsArticleAsync(
            NewsArticleDTO articleDto,
            string currentUserId
        )
        {
            articleDto.NewsArticleId = GenerateUniqueId();
            if (int.TryParse(currentUserId, out var userId))
            {
                articleDto.CreatedById = userId;
                articleDto.UpdatedById = userId;
            }
            articleDto.CreatedDate = DateTime.UtcNow;
            articleDto.ModifiedDate = DateTime.UtcNow;
            var article = mapper.Map<Repository.Entities.NewsArticle>(articleDto);
            if (articleDto.TagIds.Any())
            {
                article.Tags = await tagService.GetTagsByIdsAsync(articleDto.TagIds);
            }
            var addedNewsArticle = await articleRepository.CreateAsync(article);
            var articleDtoToReturn = mapper.Map<NewsArticleDTO>(addedNewsArticle);
            return articleDtoToReturn;
        }

        private string GenerateUniqueId()
        {
            // Tạo GUID mới
            var guid = Guid.NewGuid();

            // Tính MD5 hash của GUID (dạng chuỗi)
            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(guid.ToString()));
            // Chuyển đổi hash thành chuỗi hex
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2")); // "X2" in hoa, dùng "x2" nếu muốn chữ thường
            }
            // Lấy 6 ký tự đầu tiên của chuỗi hash
            return sb.ToString()[..6];
        }

        public async Task<int?> UpdateNewsArticleAsync(
            NewsArticleDTO articleDto,
            string currentUserId
        )
        {
            if (int.TryParse(currentUserId, out var userId))
            {
                articleDto.UpdatedById = userId;
            }
            articleDto.ModifiedDate = DateTime.UtcNow;
            var article = mapper.Map<Repository.Entities.NewsArticle>(articleDto);
            if (articleDto.TagIds.Any())
            {
                article.Tags = await tagService.GetTagsByIdsAsync(articleDto.TagIds);
            }
            var effectedRow = await articleRepository.UpdateAsync(article);
            return effectedRow;
        }

        public async Task<int?> DeleteNewsArticleAsync(string articleId)
        {
            var article = await articleRepository.GetArticleByIdAsync(articleId);
            if (article == null)
                return 0;
            article.NewsStatus = false;
            article.ModifiedDate = DateTime.Now;
            var effectedRow = await articleRepository.UpdateAsync(article);
            return effectedRow;
        }

        public async Task<
            PaginatedList<Repository.Entities.NewsArticle>
        > ListArticlesWithPaginationAndFitler(
            string? searchString,
            string? sortOrder,
            int? pageNumber,
            int? pageSize
        )
        {
            pageNumber ??= 1;
            pageSize ??= 4;
            return await articleRepository.GetCategoriesQuery(
                (int)pageNumber,
                (int)pageSize,
                searchString,
                sortOrder
            );
        }
    }
}
