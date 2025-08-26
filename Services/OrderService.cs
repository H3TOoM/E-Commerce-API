using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        // Inject main repoistory
        private readonly IMainRepoistory<Order> _mainRepoistory;
        // Inject order repoistory
        private readonly IOrderRepoistory _orderRepoistory;
        // Inject Unit Of Work
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IMainRepoistory<Order> mainRepoistory,IOrderRepoistory orderRepoistory, IUnitOfWork unitOfWork)
        {
            _mainRepoistory = mainRepoistory;   
            _orderRepoistory = orderRepoistory;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync( int userId , OrderDto dto )
        {
            // Create a new order
            var order = new Order
            {
                UserId = userId,
                TotalAmount = dto.TotalAmount,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = dto.OrderItems.Select( item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,

                } ).ToList()
            };

            // Add to DB
            await _mainRepoistory.AddAsync( order );
            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        public async Task<bool> DeleteOrderAsync( int id )
        {
            var result = await _mainRepoistory.DeleteAsync( id );
            await _unitOfWork.SaveChangesAsync();
            if (result == null)
                return false;

            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = await _orderRepoistory.GetAllAsync();
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync( int id )
        {
            var order = await _mainRepoistory.GetByIdAsync( id );
            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync( int userId )
        {
            var orders = await _orderRepoistory.GetUserOrderAsync( userId );
            return orders;
        }
    }
}
