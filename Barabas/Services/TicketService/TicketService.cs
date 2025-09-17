using Barabas.Models;
using Barabas.Repositories.TicketRepository;

namespace Barabas.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<List<Ticket>> GetAvailableTickets(int eventId)
        {
            return await _ticketRepository.GetTicketsByEventId(eventId);
        }

        public Task<Ticket> GetTicketById(int ticketId)
        {
            return _ticketRepository.GetById(ticketId);
        }

        public async Task<Ticket> ReserveTicket(int ticketId)
        {
            var ticket = await _ticketRepository.GetById(ticketId);
            if (ticket != null)
            {
                ticket.SetNotActive();
                _ = _ticketRepository.Update(ticket);
                return ticket;
            }
            return null;
        }
    }

}
