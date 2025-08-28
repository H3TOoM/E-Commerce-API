using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {

        // Inject Cart service
        private readonly ICartService _cartService;
        public CartController( ICartService cartService)
        {
            _cartService = cartService;
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




        // Get All Carts
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllCarts()
        {
            var userId = getUserIdFromClaims();
            var carts = await _cartService.GetAllCartsAsync();
            if (carts == null || !carts.Any())
            {
                return NotFound("No carts found.");
            }
            return Ok(carts);
        }




        // Get Cart By User Id
        [HttpGet("my-cart")]
        public async Task<IActionResult> GetCartByUserId()
        {
            var userId = getUserIdFromClaims();
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound("Cart not found for the user.");
            }
            return Ok(cart);
        }


        // Create New Cart
        [HttpPost]
        public async Task<IActionResult> CreateCart( int productId , int quantity)
        {
            var userId = getUserIdFromClaims();
            var cart = await _cartService.CreateCartAsync(userId, productId , quantity);
            if (cart == null)
            {
                return BadRequest("Failed to create cart. Please check the provided data.");
            }
            return CreatedAtAction(nameof(GetCartByUserId), new { id = cart.Id }, cart);
        }


        // Delete Item from Cart
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteItemFromCart(int productId)
        {
            var userId = getUserIdFromClaims();
            var result = await _cartService.DeleteCartItemAsync(userId, productId);
            if (!result)
            {
                return NotFound("Cart item not found or could not be deleted.");
            }
            return NoContent();
        }


        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = getUserIdFromClaims();
            var result = await _cartService.ClearCartAsync(userId);
            if (!result)
            {
                return NotFound("Cart could not be cleared or was already empty.");
            }
            return NoContent();
        }


    }
}
