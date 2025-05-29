using System.ComponentModel.DataAnnotations;


namespace ConsoleApp1.Domain.Entities
{
    
    public class BasketItem
    {
        [Key]
        public int BasketItemId { get; set; }
    
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    
        public int ProductId { get; set; }
        public Product Product { get; set; }
    
        public int Quantity { get; set; }
    }


    
}
