
using System.Collections.Generic;
using ConsoleApp1.Domain;

namespace ConsoleApp1.Domain;

public class Basket
{
    public int BasketId { get; set; }

    public int UserId { get; set; }
    
    public List<Product> Products { get; set; } = new List<Product>();



}