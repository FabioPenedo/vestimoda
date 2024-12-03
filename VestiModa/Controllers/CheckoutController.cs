using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using VestiModa.Models;
using VestiModa.ViewModels;

namespace VestiModa.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CartPurchase _cartPurchase;

        public CheckoutController(IConfiguration configuration, CartPurchase cartPurchase)
        {
            _configuration = configuration;
            _cartPurchase = cartPurchase;
        }

        public async Task<IActionResult> CreateCheckoutSession()
        {
            string domain = "http://localhost:5261/";

            var items = await _cartPurchase.GetCartPurchaseItemsAsync();
            _cartPurchase.Items = items;

            //Criar uma lista de itens de linha para a sessão de checkout
            var lineItems = new List<SessionLineItemOptions>();
            

            // Adiciona cada item do carrinho à sessão de checkout
            foreach (var item in _cartPurchase.Items)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // Valor em centavos
                        Currency = "brl", //moeda em real
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Amount
                });
            }
            


            // Criação da sessão de checkout
            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = domain + "Product",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }
    }
}