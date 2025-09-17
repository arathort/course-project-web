namespace Barabas.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public Order(){}
        public Order(int id, string userId, DateTime createdDate)
        {
            Id = id;
            UserId = userId;
            CreatedDate = createdDate;
        }
    }
}
