using Barabas.Models;

namespace Barabas.Services.TicketService
{
    public interface ITicketService
    {
        public Task<List<Ticket>> GetAvailableTickets(int eventId);
        public Task<Ticket> ReserveTicket(int ticketId);
        public Task<Ticket> GetTicketById(int ticketId);

    }
}
