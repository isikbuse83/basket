using System;

namespace ConsoleApp1.Domain;

public class Order
{
    public int Id { get; set; }                 
    public DateTime OrderDate { get; set; }    
    public int ProductId { get; set; }          
    public int Quantity { get; set; }           

    
    public Product Product { get; set; }
}