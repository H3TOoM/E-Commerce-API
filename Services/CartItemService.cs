using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class CartItemService: ICartItemService
    {
        private readonly ICartItemRepoistory _cartItemRepoistory;
        private readonly IUnitOfWork _unitOfWork;
        public CartItemService(ICartItemRepoistory cartItemRepoistory, IUnitOfWork unitOfWork )
        {
            _cartItemRepoistory = cartItemRepoistory;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartItem> AddItemToCartAsync( int userId, int productId, int quantity )
        {
            var cartItem = await _cartItemRepoistory.AddItemToCartAsync(userId, productId, quantity);
            await _unitOfWork.SaveChangesAsync();
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item could not be added.");
            }

            return cartItem;
        }

        public async Task<string> DeleteCartItemAsync( int userId, int cartItemId )
        {
            var result = await _cartItemRepoistory.DeleteCartItemAsync(userId, cartItemId);
            if (string.IsNullOrEmpty(result))
            {
                throw new KeyNotFoundException("Cart item could not be deleted.");
            }
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync( int userId )
        {
            var cartItems = await _cartItemRepoistory.GetCartItemsAsync(userId);

            return cartItems;
        }

        public async Task<CartItem> UpdateCartItemAsync( int userId, int cartItemId, int quantity )
        {
            var cartItem = await _cartItemRepoistory.UpdateCartItemAsync(userId, cartItemId, quantity);
            await _unitOfWork.SaveChangesAsync();
            return cartItem ?? throw new KeyNotFoundException("Cart item could not be updated.");
        }
    }
}
