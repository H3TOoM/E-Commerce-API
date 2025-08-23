using E_Commerce.Models;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {

        // Inject cartItem service
        private readonly ICartItemService _cartItemService;
        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        // Get the user ID from claims
        private int getUserIdFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier );
            if (idClaim == null)
            {
                throw new UnauthorizedAccessException( "User not authenticated" );
            }
            return int.Parse( idClaim.Value );

        }

        // get cart item by id
        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetCartItems()
        {

            var userId = getUserIdFromClaims();
            
            var cartItems = await _cartItemService.GetCartItemsAsync(userId);
            if (!cartItems.Any())
                return NotFound("No items found in the cart.");
            

            return Ok(cartItems);

        }


        // Add item to cart

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItemToCart(int productId, int quantity)
        {
            var userId = getUserIdFromClaims();
            var cartItem = await _cartItemService.AddItemToCartAsync(userId, productId, quantity);
            return Ok(cartItem);
        }


        // update cart item quantity
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, int quantity)
        {
            var userId = getUserIdFromClaims();
            var cartItem = await _cartItemService.UpdateCartItemAsync(userId, cartItemId, quantity);

            if (cartItem == null)
                return NotFound("Cart item not found.");
           

            return Ok(cartItem);
        }


        // delete cart item
        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteCartItem(int productId)
        {
            var userId = getUserIdFromClaims();        
            var result = await _cartItemService.DeleteCartItemAsync(userId, productId);
            if (string.IsNullOrEmpty(result))
                return NotFound("Cart item not found or could not be deleted.");

            return Ok("Cart item deleted successfully.");
        }
    }
}
