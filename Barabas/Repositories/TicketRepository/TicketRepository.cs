using Barabas.Data;
using Barabas.Models;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Repositories.TicketRepository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetTicketsByEventId(int eventId)
        {
            return await _context.Tickets
                                 .Where(t => t.EventId == eventId)
                                 .ToListAsync();
        }

        public async Task<Ticket> GetById(int ticketId)
        {
            return await _context.Tickets
                                 .FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task Add(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int ticketId)
        {
            var ticket = await GetById(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
