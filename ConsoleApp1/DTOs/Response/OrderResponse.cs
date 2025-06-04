using System;

namespace ConsoleApp1.DTOs.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public ProductResponse Product { get; set; }
        
        public UserResponse User { get; set; } 
    }

    
}
