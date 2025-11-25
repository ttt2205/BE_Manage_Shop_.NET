namespace Manage_Store.Models.Requests
{
    public class OrderItemReq
    {
        public int? Id { get; set; }         
        public int ProductId { get; set; }   
        public int Quantity { get; set; }    
        public decimal Price { get; set; }   
        public decimal Subtotal { get; set; } 
    }
}
