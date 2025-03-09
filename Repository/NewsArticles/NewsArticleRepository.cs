using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;

namespace Repository.NewsArticles
{
    public class NewsArticleRepository(FuNewsDbContext context) : INewsArticleRepository
    {
        public async Task<NewsArticle?> GetArticleByIdAsync(string id)
        {
            var newsArticle = await context
                .NewsArticles.Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .Include(a => a.Tags!)
                .FirstOrDefaultAsync(a => a.NewsArticleId == id);
            return newsArticle;
        }

        public async Task<IEnumerable<NewsArticle>> ListAllAsync()
        {
            var newsArticles = await context
                .NewsArticles.Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .Include(a => a.UpdatedById)
                .Include(a => a.Tags)
                .ToListAsync();
            return newsArticles;
        }

        public async Task<NewsArticle> CreateAsync(NewsArticle newsArticle)
        {
            var addedAccount = await context.NewsArticles.AddAsync(newsArticle);
            await context.SaveChangesAsync();
            return newsArticle;
        }

        public async Task<int?> UpdateAsync(NewsArticle newsArticle)
        {
            var systemAccount = await GetArticleByIdAsync(newsArticle.NewsArticleId);
            if (systemAccount == null)
            {
                return null;
            }
            await Task.Run(() => context.NewsArticles.Update(systemAccount));
            var effectedRow = await context.SaveChangesAsync();
            return effectedRow;
        }

        public async Task<int?> DeleteAsync(NewsArticle? deletedAccount)
        {
            if (deletedAccount == null)
            {
                return null;
            }
            var systemAccount = await GetArticleByIdAsync(deletedAccount.NewsArticleId);
            if (systemAccount == null)
            {
                return null;
            }
            await Task.Run(() => context.NewsArticles.Remove(systemAccount));
            var effectedRow = await context.SaveChangesAsync();
            return effectedRow;
        }
    }
}
