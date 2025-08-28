using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemsSevice _orderItemsSevice;
        public OrderItemController(IOrderItemsSevice orderItemsSevice)
        {
           _orderItemsSevice = orderItemsSevice;
        }


        // Get All Order Items
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetAllOrderItems(int orderId)
        {
            var orderItems = await _orderItemsSevice.GetItemsByOrderIdAsync(orderId);

            if (orderItems == null || !orderItems.Any())
            {
                return NotFound("No order items found for this order.");
            }

            return Ok(orderItems);
        }


        // Add Order Item
        [Authorize]
        [HttpPost]  
        public async Task<IActionResult> AddOrderItem([FromBody]  OrderItemDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Order item data is required.");
            }

           var orderItem =await _orderItemsSevice.CreateOrderItemAsync(dto);

            return Ok(orderItem);   

        }


        // Delete Order Item
        [Authorize] 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var result = await _orderItemsSevice.DeleteOrderItemAsync( id );
            if (!result)
                return NotFound();

            return NoContent();
        }


    }
}
