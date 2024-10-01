using Microsoft.EntityFrameworkCore;
using VestiModa.Context;
using VestiModa.Models;
using VestiModa.Repositories.Interfaces;

namespace VestiModa.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductAsync()
        {
            return await _context.Products.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category.Name.Equals(category))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstAsync(x => x.ProductId == id);
        }
    }
}
