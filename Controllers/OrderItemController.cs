using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderItemController(AppDbContext context)
        {
            _context = context;
        }


        // Get All Order Items
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetAllOrderItems(int orderId)
        {
            var orderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Product)
                .ToListAsync();

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

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var order = await _context.Orders.FindAsync(dto.OrderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var orderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
            };
 

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();


            return Ok(orderItem);   

        }


        // Delete Order Item
        [Authorize] 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound("Order item not found.");
            }
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return Ok("Order item deleted successfully.");
        }


    }
}
