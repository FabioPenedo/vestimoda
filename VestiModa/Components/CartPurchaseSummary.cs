using Microsoft.AspNetCore.Mvc;
using VestiModa.Models;
using VestiModa.ViewModels;

namespace VestiModa.Components
{
    public class CartPurchaseSummary : ViewComponent
    {
        private readonly CartPurchase _cartPurchase;

        public CartPurchaseSummary(CartPurchase cartPurchase)
        {
            _cartPurchase = cartPurchase;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
    }
}
