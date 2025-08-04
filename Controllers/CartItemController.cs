using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CartItemController(AppDbContext context)
        {
            _context = context;
        }

        // Get the user ID from claims
        private int? getUserIdFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (idClaim == null) return null;
            return int.Parse(idClaim.Value);
        }

        // get cart item by id
        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetCartItems()
        {

            var userId = getUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }


            // Retrieve the cart items for the user
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();

            // Check if the cart items exist
            if (cartItems == null || !cartItems.Any())
            {
                return NotFound();
            }


            return Ok(cartItems);

        }


        // Add item to cart

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddItemToCart(int productId, int quantity)
        {
            var userId = getUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }

            // Retrieve the cart for the user
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Check if the cart exists
            if (cart == null)
            {
                return NotFound("Cart not found for the user.");
            }


            // Create a new CartItem    
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            // Add the item to the cart
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }


        // update cart item quantity
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, int quantity)
        {
            var userId = getUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }




            // Retrieve the cart item
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);
            // Check if the cart item exists
            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }
            // Update the quantity
            cartItem.Quantity = quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }


        // delete cart item
        [Authorize]
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteCartItem(int productId)
        {
            var userId = getUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }

            // Retrieve the cart item
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.Cart.UserId == userId);
            // Check if the cart item exists
            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }
            // Remove the cart item
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok("Cart item deleted successfully.");
        }
    }
}
