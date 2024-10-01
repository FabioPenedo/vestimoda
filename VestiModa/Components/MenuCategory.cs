using Microsoft.AspNetCore.Mvc;
using VestiModa.Repositories.Interfaces;

namespace VestiModa.Components
{
    public class MenuCategory : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public MenuCategory(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return View(categories);
        }
    }
}
