using Barabas.Models;

namespace Barabas.Services.OrderService
{
    public interface IOrderService
    {
        public Task CreateOrder(string userId, int ticketId);
        Task<IEnumerable<OrderItem>> GetOrdersByUserId(string userId);
    }
}
