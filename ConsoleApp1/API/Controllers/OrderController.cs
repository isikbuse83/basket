using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ConsoleApp1.Services;
using System.Threading.Tasks;
using ConsoleApp1.Reponse;

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
        var (success, message) = await _orderService.CompleteOrderAsync(userId);

        if (!success)
            return BadRequest(message);

        return Ok(message);
    }

    // Eğer sipariş detaylarını almak istersen:
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