using System.Collections.Generic;
using ConsoleApp1.DTOs.Response;

public class CompleteOrderResponse
{
    public string Message { get; set; }
    public List<OrderResult> Orders { get; set; }
}