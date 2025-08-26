using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repoistries
{
    public class OrderRepoistory : IOrderRepoistory
    {
        // Inject AppDbContext 
        private readonly AppDbContext _context;
        public OrderRepoistory( AppDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _context.Orders               
                         .Include(o => o.OrderItems)
                         .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<Order>> GetUserOrderAsync( int userId )
        {
            var orders = await _context.Orders
                         .Where(o=> o.UserId == userId)
                         .Include( o => o.OrderItems )
                         .ThenInclude( ot => ot.Product )
                         .ToListAsync();

            return orders;
        }
    }
}
