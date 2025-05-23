
using System.Collections.Generic;

namespace ConsoleApp1.Informations;

public class Basket
{
    public int BasketId { get; set; }

    public int UserId { get; set; }
    
    public List<Product> Products { get; set; } = new List<Product>();



}