using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repoistries
{
    public class CartRepoistory : ICartRepoistory
    {
        // Inject AppDbContext
        private readonly AppDbContext _context;
        public CartRepoistory( AppDbContext context )
        {
            _context = context;
        }

        public async Task<bool> ClearCartAsync( int userId )
        {
            var cart = await _context.Carts.FirstOrDefaultAsync( c => c.UserId == userId );
            if (cart == null)
                return false;

            _context.Carts.Remove( cart );
            return true;
        }

        public async Task<Cart> CreateCartAsync( int userId, int productId, int quantity )
        {
            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity
                    }
                }
            };

            await _context.Carts.AddAsync( cart );
            return cart;
        }

        public async Task<bool> DeleteCartItemAsync( int userId, int productId )
        {
            var cart = await _context.Carts
                .Include( c => c.Items )
                .FirstOrDefaultAsync( c => c.UserId == userId );

            if (cart == null)
                return false;

            var cartItem = cart.Items.FirstOrDefault( ci => ci.ProductId == productId );
            if (cartItem == null)
                return false;

            cart.Items.Remove( cartItem );
            return true;
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            var carts = await _context.Carts
                .Include( c => c.Items )
                .ThenInclude( ci => ci.Product )
                .ToListAsync();

            return carts;
        }

        public async Task<Cart> GetCartByUserIdAsync( int userId )
        {
            var cart = await _context.Carts
                .Include( c => c.Items )
                .ThenInclude( ci => ci.Product )
                .FirstOrDefaultAsync( c => c.UserId == userId );

            return cart;
        }
    }
}
