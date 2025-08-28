using E_Commerce.Models;

namespace E_Commerce.Repoistries.Base
{
    public interface IOrderItemRepoistory
    {
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);        
    }
}
