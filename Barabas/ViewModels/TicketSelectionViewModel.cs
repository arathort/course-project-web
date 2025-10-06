namespace Barabas.Models
{
    public class TicketSelectionViewModel
    {
        public string EventName { get; set; }
        public List<int> AllSeats { get; set; } = [];
        public List<int> AvailableSeats { get; set; }
        public List<int> AvailableTicketIds { get; set; }
        public List<int> TakenSeats { get; set; } = [];

        public List<Ticket> Cart { get; set; } = new List<Ticket>();
    }

}
