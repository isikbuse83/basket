using System.Collections.Generic;

namespace ConsoleApp1.DTOs.Response;

public class BasketResponse
{
    public int BasketId { get; set; }
    public List<BasketItemResponse> BasketItems { get; set; }
}