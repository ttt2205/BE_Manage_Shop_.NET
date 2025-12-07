namespace Manage_Store.Models.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "pending";
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }

        public CustomerDto? Customer { get; set; }
        public UserDto? User { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
