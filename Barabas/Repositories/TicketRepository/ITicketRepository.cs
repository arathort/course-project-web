using Barabas.Models;

namespace Barabas.Repositories.TicketRepository
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetTicketsByEventId(int eventId);
        Task<Ticket> GetById(int ticketId);
        Task Add(Ticket ticket);
        Task Delete(int ticketId);
        Task Update(Ticket ticket);
    }
}
