using ConsoleApp1.Reponse;

namespace ConsoleApp1.Reponse
{
    public class BasketItemResponse
    {
        public int BasketItemId { get; set; }
        public int Quantity { get; set; }
        public ProductResponse Product { get; set; }
    }
    
}
