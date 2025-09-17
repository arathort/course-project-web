using Barabas.Models;

namespace Barabas.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task AddOrderItem(OrderItem orderItem);
        Task<List<OrderItem>> GetOrdersByUserId(string userId);
        Task<Order> GetById(int orderId);
        Task Delete(int orderId);
    }
}
