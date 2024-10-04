using Microsoft.EntityFrameworkCore;
using VestiModa.Context;

namespace VestiModa.Models
{
    public class CartPurchase
    {
        private readonly AppDbContext _context;

        public CartPurchase(AppDbContext context)
        {
            _context = context;
        }

        public string CartPurchaseId { get; set; } = string.Empty;
        public List<CartPurchaseItem>? Items { get; set; }


        public static CartPurchase GetCart(IServiceProvider services)
        {
            //define uma sessão
            var httpContextAccessor =
                services.GetRequiredService<IHttpContextAccessor>();

            if (httpContextAccessor == null)
            {
                throw new InvalidOperationException("IHttpContextAccessor is not available.");
            }

            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            var session = httpContext.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }


            //obtem um serviço do tipo do nosso contexto
            var context = services.GetService<AppDbContext>() ??
                throw new InvalidOperationException("The AppDbContext service is not registered.");


            //obtem ou gera o Id do carrinho
            string carrinhoId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            //atribui o id do carrinho na Sessão
            session.SetString("CartId", carrinhoId);

            return new CartPurchase(context)
            {
                CartPurchaseId = carrinhoId
            };
        }

        public async Task AddToCartAsync(Product product)
        {
            var cartPurchaseItem = await _context.CartPurchaseItems
                .SingleOrDefaultAsync(x => x.Product.ProductId == product.ProductId &&
                                            x.CartPurchaseId == CartPurchaseId);

            if (cartPurchaseItem == null)
            {
                cartPurchaseItem = new CartPurchaseItem
                {
                    CartPurchaseId = CartPurchaseId,
                    Product = product,
                    Amount = 1
                };
                await _context.CartPurchaseItems.AddAsync(cartPurchaseItem);
            }
            else
            {
                cartPurchaseItem.Amount++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(Product product)
        {
            var cartPurchaseItem = await _context.CartPurchaseItems
                .SingleOrDefaultAsync(x => x.Product.ProductId == product.ProductId &&
                                            x.CartPurchaseId == CartPurchaseId);

            if (cartPurchaseItem != null)
            {
                if (cartPurchaseItem.Amount > 1)
                {
                    cartPurchaseItem.Amount--;
                }
                else
                {
                    _context.CartPurchaseItems.Remove(cartPurchaseItem);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<CartPurchaseItem>> GetCartPurchaseItemsAsync()
        {
            return Items ??
                (Items = await _context.CartPurchaseItems
                .Where(c => c.CartPurchaseId == CartPurchaseId)
                .Include(c => c.Product)
                .ToListAsync());
        }

        public async Task CleanCart()
        {
           var cartItems = await _context.CartPurchaseItems.Where(c => c.CartPurchaseId == CartPurchaseId).ToListAsync();

           _context.CartPurchaseItems.RemoveRange(cartItems);
           await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetCartTotalPurchase()
        {
            return await _context.CartPurchaseItems
                .Where(c => c.CartPurchaseId == CartPurchaseId)
                .Select(c => c.Product.Price * c.Amount)
                .SumAsync();
        }
    }
}

