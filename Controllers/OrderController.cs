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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
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

        // Get All Orders 
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllOrders()
        {
            var ordres = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return Ok(ordres);
        }


        // get My Orders 
        [Authorize]
        [HttpGet("myOrders")]
        public async Task<IActionResult> getMyOrders()
        {

            var userId = getUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }


            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();


            return Ok(orders);

        }


        // Add Order
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> addOrder([FromBody] OrderDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Order data is required.");
            }


            var userId = getUserIdFromClaims();
            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }


            // Create a new order
            var order = new Order
            {
                UserId = (int)userId,
                TotalAmount = dto.TotalAmount,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = dto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,

                }).ToList()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok(order);

        }


        // Delete Order 
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = getUserIdFromClaims();

            var order = await _context.Orders
                             .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found or not authorized to delete.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(order);


        }






    }
}