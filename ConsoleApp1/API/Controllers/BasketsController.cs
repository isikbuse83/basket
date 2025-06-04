using System.Threading.Tasks;
using AutoMapper;
using ConsoleApp1.Application.Services;
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

        if (basket == null)
            return NotFound("Sepet bulunamadı");

        var basketDto = _mapper.Map<BasketResponse>(basket);
        return Ok(basketDto);
    }

    [HttpPost("add/{productId:int}")]
    public async Task<ActionResult<string>> AddToBasket(int userId, int productId)
    {
        var result = await _basketService.AddToBasketAsync(userId, productId);

        if (result.Contains("hata") || result.Contains("bulunamadı") || result.Contains("Yetersiz"))
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("remove/{productId:int}")]
    public async Task<ActionResult<string>> RemoveFromBasket(int userId, int productId)
    {
        var result = await _basketService.RemoveFromBasketAsync(userId, productId);

        if (result.Contains("bulunamadı"))
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("clear")]
    public async Task<ActionResult<string>> ClearBasket(int userId)
    {
        var result = await _basketService.CleanBasketAsync(userId);

        if (result.Contains("bulunamadı"))
            return NotFound(result);

        return Ok(result);
    }
}