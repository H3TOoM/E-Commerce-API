using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        public OrderController( IOrderService orderService )
        {
            _orderService = orderService;
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




        [Authorize( Roles = "admin" )]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                if (orders == null || !orders.Any())
                    return NotFound( "No Orders yet!" );

                return Ok( orders );

            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message );
            }
        }


        [Authorize]
        [HttpGet( "my-orders" )]
        public async Task<IActionResult> GetMyOrders()
        {
            try
            {
                var userId = getUserIdFromClaims();
                var orders = await _orderService.GetUserOrdersAsync( userId );
                if (orders == null || !orders.Any())
                    return NotFound( "No Orders yet!" );

                return Ok( orders );

            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message );
            }
        }


        // Get Order By Id
        [Authorize]
        [HttpGet( "{orderId}" )]
        public async Task<IActionResult> GetOrderById(int orderId )
        {
            var order = await _orderService.GetOrderByIdAsync( orderId );
            if (order == null)
                return NotFound();

            return Ok( order );
        }


        // Create Order
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder( OrderDto dto )
        {
            var userId = getUserIdFromClaims();
            var order = await _orderService.CreateOrderAsync( userId, dto );
            if (order == null)
                return NotFound();

            return Ok( order );
        }



        [Authorize(Roles = "admin")]
        [HttpDelete( "{orderId}" )]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            if(!result)
                return NotFound();

            return NoContent();
        }
        


    }
}