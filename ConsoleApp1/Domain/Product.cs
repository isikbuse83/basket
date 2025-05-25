namespace ConsoleApp1.Domain;

public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStock { get; private set; }


    public Product SetStock(int newStock)
    {
        ProductStock = newStock;
        return this;
    }


    public void DecreaseStock()
    {
        ProductStock -= 1;
    }

    public void IncreaseStock()
    {
        ProductStock += 1;
    }
    
}

