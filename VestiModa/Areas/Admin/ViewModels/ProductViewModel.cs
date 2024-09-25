using VestiModa.Models;

namespace VestiModa.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product, IEnumerable<Category> categories, List<string?>? imageNames)
        {
            Product = product;
            Categories = categories;
            ImageNames = imageNames;
        }
        public ProductViewModel(Product product, IEnumerable<Category> categories)
        {
            Product = product;
            Categories = categories;
        }

        public ProductViewModel(IEnumerable<Category> categories, List<string?>? imageNames)
        {
            Categories = categories;
            ImageNames = imageNames;
        }

        public ProductViewModel() { }


        public Product Product { get; set; } = null!;
        public IEnumerable<Category> Categories { get; set; } = null!;
        public List<string?>? ImageNames { get; set; }
    }
}
