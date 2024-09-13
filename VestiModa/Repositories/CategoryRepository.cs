using Microsoft.EntityFrameworkCore;
using VestiModa.Context;
using VestiModa.Models;
using VestiModa.Repositories.Interfaces;

namespace VestiModa.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstAsync(x => x.CategoryId == id);
        }
    }
}
