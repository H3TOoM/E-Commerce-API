using E_Commerce.Models;

namespace E_Commerce.Repoistries.Base
{
    public interface ICartItemRepoistory
    {
      
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId);

        Task<CartItem> AddItemToCartAsync(int userId, int productId, int quantity);

        Task<CartItem> UpdateCartItemAsync(int userId, int cartItemId, int quantity);

        Task<string> DeleteCartItemAsync(int userId ,int cartItemId);

    }
}
