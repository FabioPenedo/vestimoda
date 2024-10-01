using VestiModa.Models;

namespace VestiModa.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductAsync();
        Task<IEnumerable<Product>> GetProductsByNameAsync(string category);
        Task<Product> GetProductByIdAsync(int id);
    }
}
