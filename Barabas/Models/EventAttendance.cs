using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barabas.Models
{
    public class EventAttendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; } 

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } 
    }
}
