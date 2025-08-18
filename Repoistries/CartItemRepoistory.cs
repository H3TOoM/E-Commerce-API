using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repoistries
{
    public class CartItemRepoistory : ICartItemRepoistory
    {

        // Inject the DbContext
        private readonly AppDbContext _context;
        public CartItemRepoistory( AppDbContext context )
        {
            _context = context;
        }



        // Get all cart items for a user
        public async Task<IEnumerable<CartItem>> GetCartItemsAsync( int userId )
        {
            var cartItems = await _context.CartItems
                .Include( ci => ci.Product )
                .Where( ci => ci.Cart.UserId == userId )
                .ToListAsync();


            return cartItems;
        }


        // Add an item to the cart
        public async Task<CartItem> AddItemToCartAsync( int userId, int productId, int quantity )
        {
            // Retrieve the cart for the user
            var cart = await _context.Carts
                .Include( c => c.Items )
                .FirstOrDefaultAsync( c => c.UserId == userId );

            // Check if the cart exists
            if (cart == null)
            {
                throw new KeyNotFoundException( "Cart not found for the user." );
            }

            // Check if the product exists
            var product = await _context.Products.FindAsync( productId );
            if (product == null)
            {
                throw new KeyNotFoundException( "Product not found." );
            }

            var cartItem = new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                CartId = cart.Id,
                Product = product
            };

            // Add the cart item to the cart
            await _context.CartItems.AddAsync( cartItem );
            return cartItem;
        }

        // Update an existing cart item
        public async Task<CartItem> UpdateCartItemAsync( int userId, int cartItemId, int quantity )
        {
            var cartItem = await _context.CartItems
                .Include( ci => ci.Cart )
                .FirstOrDefaultAsync( ci => ci.Id == cartItemId && ci.Cart.UserId == userId );

            if (cartItem == null)
            {
                throw new KeyNotFoundException( "Cart item not found." );
            }

            if (quantity <= 0)
            {
                throw new ArgumentException( "Quantity must be greater than zero." );
            }

            cartItem.Quantity = quantity;
            _context.CartItems.Update( cartItem );
            return cartItem;
        }

        public async Task<string> DeleteCartItemAsync( int userId, int cartItemId )
        {
            var cartItem = await _context.CartItems
                .Include( ci => ci.Cart )
                .FirstOrDefaultAsync( ci => ci.Id == cartItemId && ci.Cart.UserId == userId );

            if (cartItem == null)
                {
                throw new KeyNotFoundException( "Cart item not found." );
            }

            _context.CartItems.Remove( cartItem );
            return "Cart item deleted successfully.";
        }



    }
}
