using VestiModa.Models;

namespace VestiModa.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product, IEnumerable<Category> categories)
        {
            Product = product;
            Categories = categories;
        }

        public ProductViewModel(IEnumerable<Category> categories)
        {
            Categories = categories;
        }

        public ProductViewModel() { }


        public Product Product { get; set; } = null!;
        public IEnumerable<Category> Categories { get; set; } = null!;
    }
}
