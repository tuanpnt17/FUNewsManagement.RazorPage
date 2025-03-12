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
            var existingArticle = await context
                .NewsArticles.Include(a => a.Tags)
                .FirstOrDefaultAsync(a => a.NewsArticleId == newsArticle.NewsArticleId);

            if (existingArticle == null)
            {
                return null;
            }

            // Update properties
            existingArticle.NewsTitle = newsArticle.NewsTitle;
            existingArticle.Headline = newsArticle.Headline;
            existingArticle.NewsContent = newsArticle.NewsContent;
            existingArticle.NewsSource = newsArticle.NewsSource;
            existingArticle.CategoryId = newsArticle.CategoryId;
            existingArticle.NewsStatus = newsArticle.NewsStatus;
            existingArticle.UpdatedById = newsArticle.UpdatedById;
            existingArticle.ModifiedDate = DateTime.UtcNow;

            // Handle tags
            var existingTags = existingArticle.Tags.ToList();
            foreach (var tag in existingTags)
            {
                if (newsArticle.Tags.All(t => t.TagId != tag.TagId))
                {
                    existingArticle.Tags.Remove(tag);
                }
            }

            foreach (var tag in newsArticle.Tags)
            {
                if (existingArticle.Tags.All(t => t.TagId != tag.TagId))
                {
                    existingArticle.Tags.Add(tag);
                }
            }

            context.NewsArticles.Update(existingArticle);
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
