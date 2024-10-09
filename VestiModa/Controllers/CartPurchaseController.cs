using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VestiModa.Models;
using VestiModa.Repositories.Interfaces;
using VestiModa.ViewModels;

namespace VestiModa.Controllers
{
    public class CartPurchaseController : Controller
    {
        private readonly CartPurchase _cartPurchase;
        private readonly IProductRepository _productRepository;

        public CartPurchaseController(CartPurchase cartPurchase, IProductRepository productRepository)
        {
            _cartPurchase = cartPurchase;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _cartPurchase.GetCartPurchaseItemsAsync();
            _cartPurchase.Items = items;

            var cartTotalPurchase = await _cartPurchase.GetCartTotalPurchase();

            var cartPurchaseVM = new CartPurchaseViewModel
            {
                CartPurchase = _cartPurchase,
                CartPurchaseTotal = cartTotalPurchase
            };

            return View(cartPurchaseVM);
        }

        [Authorize]
        public async Task<IActionResult> AddItemToCart(int productId)
        {
            var selectedProduct = await _productRepository.GetProductByIdAsync(productId);
            if (selectedProduct != null)
            {
                await _cartPurchase.AddToCartAsync(selectedProduct);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> RemoveItemToCart(int productId)
        {
            var selectedProduct = await _productRepository.GetProductByIdAsync(productId);
            if (selectedProduct != null)
            {
                await _cartPurchase.RemoveFromCartAsync(selectedProduct);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
