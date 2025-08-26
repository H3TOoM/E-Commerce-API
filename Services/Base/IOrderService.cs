using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);

        Task<Order> GetOrderByIdAsync(int id);

        Task<Order> CreateOrderAsync(int userId, OrderDto dto );

        Task<bool> DeleteOrderAsync(int id);
    }
}
