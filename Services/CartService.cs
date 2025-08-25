using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class CartService : ICartService
    {
        // Inject CartRepoistory
        private readonly ICartRepoistory _cartRepoistory;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(ICartRepoistory cartRepoistory, IUnitOfWork unitOfWork )
        {
            _cartRepoistory = cartRepoistory;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ClearCartAsync( int userId )
        {
            var result = await _cartRepoistory.ClearCartAsync( userId );
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Cart> CreateCartAsync( int userId, int productId, int quantity )
        {
            var cart = await _cartRepoistory.CreateCartAsync( userId, productId, quantity );
            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<bool> DeleteCartItemAsync( int userId, int productId )
        {
            var result = await _cartRepoistory.DeleteCartItemAsync( userId, productId );
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            var carts = await _cartRepoistory.GetAllCartsAsync();
            return carts;
        }

        public async Task<Cart> GetCartByUserIdAsync( int userId )
        {
            var cart = await _cartRepoistory.GetCartByUserIdAsync( userId );
            return cart;
        }
    }
}
