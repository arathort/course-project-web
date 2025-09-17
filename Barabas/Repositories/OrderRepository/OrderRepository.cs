using Barabas.Data;
using Barabas.Models;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderItem>> GetOrdersByUserId(string userId)
        {
            return await _context.OrderItems
                                 .Where(o => o.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Order> GetById(int orderId)
        {
            return await _context.Orders
                                 .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task Delete(int orderId)
        {
            var order = await GetById(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
