using E_Commerce.Models;

namespace E_Commerce.Repoistries.Base
{
    public interface ICartRepoistory
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<Cart> CreateCartAsync( int userId ,int productId, int quantity );

        Task<bool> DeleteCartItemAsync(int userId , int productId);

        Task<bool> ClearCartAsync(int userId);

    }
}
