using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using VestiModa.ViewModels;

namespace VestiModa.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _configuration;

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult CreateCheckoutSession(CartPurchaseViewModel cart)
        {
            string domain = "https://localhost:7236/";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 5000, // Valor em centavos (50.00 BRL)
                            Currency = "brl",  // Mudança para real
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Stubborn Attachments",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = domain + "/success.html",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }
    }
}
