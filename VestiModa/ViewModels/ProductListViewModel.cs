using VestiModa.Models;

namespace VestiModa.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public string CurrentCategory { get; set; } = string.Empty;
    }
}
