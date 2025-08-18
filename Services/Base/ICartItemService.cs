using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface ICartItemService
    {

        Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId);
        Task<CartItem> AddItemToCartAsync(int userId, int productId, int quantity);
        Task<CartItem> UpdateCartItemAsync(int userId, int cartItemId, int quantity);
        Task<string> DeleteCartItemAsync(int userId, int cartItemId);

    }
}
