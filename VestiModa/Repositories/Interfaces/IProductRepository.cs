using VestiModa.Models;

namespace VestiModa.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductAsync();
        Task<Product> GetProductByIdAsync(int id);
    }
}
