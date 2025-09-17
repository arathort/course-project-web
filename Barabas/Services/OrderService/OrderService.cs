using Barabas.Models;
using Barabas.Repositories.OrderRepository;
using Barabas.Services.TicketService;

namespace Barabas.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITicketService _ticketService;

        public OrderService(IOrderRepository orderRepository, ITicketService ticketService)
        {
            _orderRepository = orderRepository;
            _ticketService = ticketService;
        }

        public async Task CreateOrder(string userId, int ticketId)
        {
            var ticket = await _ticketService.ReserveTicket(ticketId);
            if (ticket != null)
            {
                var orderItem = new OrderItem(0, userId, ticketId);
                await _orderRepository.AddOrderItem(orderItem);
            }
        }

        public async Task<IEnumerable<OrderItem>> GetOrdersByUserId(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userId);
            return [.. orders];
        }
    }
}