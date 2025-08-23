using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }


        // Get the user ID from claims
        private int? getUserIdFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null) return null;
            return int.Parse(idClaim.Value);
        }



        // get cart by user id
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCartByUserId()
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
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Check if the cart exists
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
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


            // Retrieve the user's cart
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);


            if (cart == null)
            {
                // Create a new cart if it doesn't exist
                cart = new Cart { UserId = (int)userId, Items = new List<CartItem>() };
                _context.Carts.Add(cart);
            }

            // Check if the item already exists in the cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                // Update the quantity of the existing item
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            await _context.SaveChangesAsync();
            return Ok(cart);
        }


        // Remove item from cart
        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveItemFromCart(int productId)
        {
            var userId = getUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }

            // Retrieve the user's cart
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Check if the cart exists
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }
            // Find the item to remove
            var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove == null)
            {
                return NotFound("Item not found in cart.");
            }
            // Remove the item from the cart
            cart.Items.Remove(itemToRemove);
            await _context.SaveChangesAsync();
            return Ok(cart);
        }


        // Clear cart
        [Authorize]
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = getUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }

            // Retrieve the user's cart
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            // Check if the cart exists
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }
            // Clear the items in the cart
            cart.Items.Clear();
            await _context.SaveChangesAsync();
            return Ok(cart);
        }
    }
}
