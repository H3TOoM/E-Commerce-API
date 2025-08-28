using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repoistries
{
    public class OrderItemRepoistory : IOrderItemRepoistory
    {
        // Inject AppDbcontext
        private readonly AppDbContext _context;
        public OrderItemRepoistory( AppDbContext context )
        {
            _context = context;
        }

       

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync( int orderId )
        {

            var orderItems = await _context.OrderItems
                .Where( oi => oi.OrderId == orderId )
                .Include( oi => oi.Product )
                .ToListAsync();

            return orderItems;
        }
    }
}
