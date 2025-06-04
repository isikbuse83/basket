using System.Threading.Tasks;
using AutoMapper;
using ConsoleApp1.Application.Services;
using ConsoleApp1.DTOs.Request;
using ConsoleApp1.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleApp1.API.Controllers;

[ApiController]
[Route("api/[controller]/{userId:int}")]
public class BasketsController : ControllerBase
{
    private readonly BasketService _basketService;
    private readonly IMapper _mapper;

    public BasketsController(BasketService basketService, IMapper mapper)
    {
        _basketService = basketService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<BasketResponse>> GetBasket(int userId)
    {
        var basket = await _basketService.GetBasketAsync(userId);

        var basketDto = _mapper.Map<BasketResponse>(basket);
        return Ok(basketDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddToBasket(int userId, [FromBody] AddToBasketRequest request)
    {
        await _basketService.AddToBasketAsync(userId, request.ProductId);

        return Ok();
    }

    [HttpDelete("remove/{productId:int}")]
    public async Task<ActionResult> RemoveFromBasket(int userId, int productId)
    {
         await _basketService.RemoveFromBasketAsync(userId, productId);

        return Ok();
    }

    [HttpDelete("clear")]
    public async Task<ActionResult> ClearBasket(int userId)
    {
         await _basketService.CleanBasketAsync(userId);
         
        return Ok();
    }
}