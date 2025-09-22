using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barabas.Models
{
    public class Event
    {
        public int Id { get;  set; } 
        public string Name { get;  set; } 
        public string Description { get;  set; } 
        public string Location { get;  set; } 
        public DateTime Date { get; set; } 
        public string? Image { get; set; } 
        [Required]
        public int CreatedBy { get; set; }
        public float Price { get; set; }

        [ForeignKey("EventCategoryId")]
        public int EventCategoryId { get; set; }

        public Event()
        {}

        public Event(int id,string name, string description, DateTime date, string location, string image, int createdBy)
        {
            Id = id;
            Name = name;
            Description = description;
            Location = location;
            Date = date;
            Image = image;
            CreatedBy = createdBy;
        }
    }
}
