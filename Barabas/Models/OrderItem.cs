namespace Barabas.Models
{
    public class OrderItem
    {
        public OrderItem(int id, string userId, int ticketId)
        {
            Id = id;
            UserId = userId;
            TicketId = ticketId;
        }

        public OrderItem() { }

        public int Id { get; set; } 
        public string UserId { get; set; } 
        public int TicketId { get; set; } 

        
    }
}
