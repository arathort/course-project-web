namespace Barabas.ViewModels
{
    public class PurchasedTicketViewModel
    {
        public int TicketId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public int SeatNumber { get; set; }
        public float Price { get; set; }
    }

}
