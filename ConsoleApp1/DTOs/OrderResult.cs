namespace ConsoleApp1.DTOs.Response
{
    public class OrderResult
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}