using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface IOrderItemsSevice
    {
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
        Task<OrderItem> CreateOrderItemAsync(OrderItemDto dto);

        Task<bool> DeleteOrderItemAsync(int id);

    }
}
