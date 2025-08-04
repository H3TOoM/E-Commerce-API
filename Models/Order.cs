namespace E_Commerce.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
