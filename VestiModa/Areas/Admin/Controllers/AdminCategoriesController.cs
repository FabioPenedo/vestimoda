using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VestiModa.Context;
using VestiModa.Models;
using VestiModa.Repositories.Interfaces;

namespace VestiModa.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public AdminCategoriesController(AppDbContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId, Name, Description")] Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
