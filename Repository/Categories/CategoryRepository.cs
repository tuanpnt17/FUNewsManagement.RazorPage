﻿using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;

namespace Repository.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FuNewsDbContext _context;

        public CategoryRepository(FuNewsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> ListAllAsync()
        {
            var categories = await _context
                .Categories.Include(c => c.NewsArticles)
                .Include(c => c.ParentCategory)
                .ToListAsync();
            return categories;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category.ParentCategoryId == 0)
                category.ParentCategoryId = null;
            var addedAccount = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return addedAccount.Entity;
        }

        public async Task<int?> UpdateAsync(Category category)
        {
            var updatedCategory = await GetCategoryByIdAsync(category.CategoryId);
            if (updatedCategory == null)
                return 0;

            if (category.CategoryName != updatedCategory.CategoryName)
                updatedCategory.CategoryName = category.CategoryName;

            if (category.CategoryDesciption != updatedCategory.CategoryDesciption)
                updatedCategory.CategoryDesciption = category.CategoryDesciption;

            if (category.IsActive != updatedCategory.IsActive)
                updatedCategory.IsActive = category.IsActive;

            if (category.ParentCategoryId != updatedCategory.ParentCategoryId)
            {
                updatedCategory.ParentCategoryId =
                    category.ParentCategoryId == 0 ? null : category.ParentCategoryId;
            }

            _context.Categories.Update(updatedCategory);
            var effectedRow = await _context.SaveChangesAsync();
            return effectedRow;
        }

        public async Task<int?> DeleteAsync(Category? category)
        {
            if (category == null)
                return null;
            var deletedCategory = await GetCategoryByIdAsync(category.CategoryId);
            if (deletedCategory == null)
            {
                return null;
            }
            await Task.Run(() => _context.Categories.Remove(deletedCategory));
            var effectedRow = await _context.SaveChangesAsync();
            return effectedRow;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context
                .Categories.Include(c => c.NewsArticles)
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }
    }
}
