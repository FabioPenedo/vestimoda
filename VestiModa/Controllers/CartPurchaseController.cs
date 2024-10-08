﻿using Microsoft.AspNetCore.Mvc;
using VestiModa.Models;
using VestiModa.ViewModels;

namespace VestiModa.Controllers
{
    public class CartPurchaseController : Controller
    {
        private readonly CartPurchase _cartPurchase;

        public CartPurchaseController(CartPurchase cartPurchase)
        {
            _cartPurchase = cartPurchase;
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
    }
}