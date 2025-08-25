using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface ICartService
    {
        Task<bool> ClearCartAsync(int userId);
        Task<bool> DeleteCartItemAsync(int userId, int productId);
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> CreateCartAsync(int userId, int productId, int quantity);
    }
}
