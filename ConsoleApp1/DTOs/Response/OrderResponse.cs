using System;
using ConsoleApp1.Reponse;

namespace ConsoleApp1.Reponse
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public ProductResponse Product { get; set; }
    }

    
}
