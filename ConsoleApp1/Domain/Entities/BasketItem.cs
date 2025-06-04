using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Domain.Entities
{
    [Owned]
    public class BasketItem
    {
        public int ProductId { get; private set; }
        public Product Product { get; set; }
    
        public int Quantity { get; private set; }

        public BasketItem(int productId)
        {
            ProductId = productId;
        }

        public void IncreaseQuantity()
        {
            Quantity++;
        }
    }
}
