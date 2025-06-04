namespace ConsoleApp1.DTOs.Request;

public class ProductCreateRequest
{
    public string ProductName { get; set; }
    
    public decimal Price { get; set; }
    public int WarehouseStock { get; set; }
}