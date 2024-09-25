using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VestiModa.Areas.Admin.ViewModels;
using VestiModa.Context;
using VestiModa.Models;
using VestiModa.Repositories;
using VestiModa.Repositories.Interfaces;

namespace VestiModa.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ConfigurationImages _myConfig;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminProductsController(AppDbContext context, 
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository,
            IWebHostEnvironment hostEnvironment,
            IOptions<ConfigurationImages> myConfiguration)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = hostEnvironment;
            _myConfig = myConfiguration.Value;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetProductAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            string userImagesPath = Path.Combine(_webHostEnvironment.WebRootPath, _myConfig.NomePastaImagensProdutos);
            var imageNames = Directory.GetFiles(userImagesPath).Select(Path.GetFileName).ToList();

            var categories = await _categoryRepository.GetCategoriesAsync();
            var viewModel = new ProductViewModel(categories, imageNames);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId, Name, Description, Price, StockQuantity, ImageName, CategoryId")] Product product)
        {
            if(ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetCategoriesAsync();

            string userImagesPath = Path.Combine(_webHostEnvironment.WebRootPath, _myConfig.NomePastaImagensProdutos);
            var imageNames = Directory.GetFiles(userImagesPath).Select(Path.GetFileName).ToList();

            var viewModel = new ProductViewModel(product, categories, imageNames);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            var product = await _productRepository.GetProductByIdAsync(id);
            var viewModel = new ProductViewModel(product, categories);    
            if (product == null)
            {
                ViewData["Erro"] = "O ID do usuário não foi encontrado";
                return View("NotFound");
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId, Name, Description, Price, StockQuantity, CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                ViewData["Erro"] = "O ID do usuário não foi encontrado";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetCategoriesAsync();
            var viewModel = new ProductViewModel(product, categories);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                ViewData["Erro"] = "O ID do usuário não foi encontrado";
                return View("NotFound");
            }
            return View(product);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                ViewData["Erro"] = "O ID do usuário não foi encontrado";
                return View("NotFound");
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                ViewData["Erro"] = "O ID do usuário não foi encontrado";
                return View("NotFound");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
