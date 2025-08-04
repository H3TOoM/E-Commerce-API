namespace E_Commerce.DTOs
{
    public class OrderDto
    {
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public required List<OrderItemDto> OrderItems { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
