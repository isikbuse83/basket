using System.Threading.Tasks;
using AutoMapper;
using ConsoleApp1.Application.Services;
using ConsoleApp1.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleApp1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(OrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }
    
    [HttpPost("complete/{userId}")]
    public async Task<ActionResult> CompleteOrder(int userId)
    {
        var result = await _orderService.CompleteOrderAsync(userId);

        if (result.Orders == null || result.Orders.Count == 0)
            return BadRequest(result.Message);

        return Ok(result);
    }
    
    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);

        if (order == null)
            return NotFound("Sipariş bulunamadı");

        var orderDto = _mapper.Map<OrderResponse>(order);
        return Ok(orderDto);
    }

}