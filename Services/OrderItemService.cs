using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class OrderItemService : IOrderItemsSevice
    {
        // Inject Repoistories
        private readonly IMainRepoistory<OrderItem> _mainRepoistory;
        private readonly IOrderItemRepoistory _orderItemRepoistory;
        private readonly IUnitOfWork _unitOfWork;
        public OrderItemService( IMainRepoistory<OrderItem> mainRepoistory, IOrderItemRepoistory orderItemRepoistory, IUnitOfWork unitOfWork )
        {
            _mainRepoistory = mainRepoistory;
            _orderItemRepoistory = orderItemRepoistory;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderItem> CreateOrderItemAsync( OrderItemDto dto )
        {

                
            var orderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
            };

            await _mainRepoistory.AddAsync( orderItem );
            await _unitOfWork.SaveChangesAsync();

            return orderItem;
        }

        public async Task<bool> DeleteOrderItemAsync( int id )
        {
            var item = await _mainRepoistory.GetByIdAsync( id );
            if (item == null)
                return false;
                
            

            await _mainRepoistory.DeleteAsync( id );
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync( int orderId )
        {
            var items = await _orderItemRepoistory.GetOrderItemsByOrderIdAsync(orderId);    
            return items;
        }
    }
}
