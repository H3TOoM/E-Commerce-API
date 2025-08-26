using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Repoistries.Base
{
    public interface IOrderRepoistory
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetUserOrderAsync(int userId);
    }
}
