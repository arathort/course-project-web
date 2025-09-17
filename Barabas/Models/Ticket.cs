using System.ComponentModel.DataAnnotations;

namespace Barabas.Models
{
    public class Ticket
    {
        public Ticket(int id, int eventId, int seatNumber)
        {
            Id = id;
            EventId = eventId;
            SeatNumber = seatNumber;
            IsActive = true;
        }

        public Ticket() { }

        public int Id { get; set; }
        [Required]
        public int EventId { get; set; }
        public int SeatNumber { get; set; } 
        public bool IsActive { get; set; }

        public void SetNotActive() 
        { 
            IsActive = false;
        }

        public void SetActive()
        {
            IsActive = true;
        }
    }
}
