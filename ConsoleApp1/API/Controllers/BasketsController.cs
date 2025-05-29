using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ConsoleApp1.Services;
using System.Threading.Tasks;
using ConsoleApp1.Reponse;

[ApiController]
[Route("api/[controller]")]
public class BasketsController : ControllerBase
{
    private readonly BasketService _basketService;
    private readonly IMapper _mapper;

    public BasketsController(BasketService basketService, IMapper mapper)
    {
        _basketService = basketService;
        _mapper = mapper;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<BasketResponse>> GetBasket(int userId)
    {
        var basket = await _basketService.GetBasketAsync(userId);

        if (basket == null)
            return NotFound("Sepet bulunamadı");

        var basketDto = _mapper.Map<BasketResponse>(basket);
        return Ok(basketDto);
    }

    [HttpPost("{userId}/add/{productId}")]
    public async Task<ActionResult<string>> AddToBasket(int userId, int productId)
    {
        var result = await _basketService.AddToBasketAsync(userId, productId);

        if (result.Contains("hata") || result.Contains("bulunamadı") || result.Contains("Yetersiz"))
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{userId}/remove/{productId}")]
    public async Task<ActionResult<string>> RemoveFromBasket(int userId, int productId)
    {
        var result = await _basketService.RemoveFromBasketAsync(userId, productId);

        if (result.Contains("bulunamadı"))
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{userId}/clear")]
    public async Task<ActionResult<string>> ClearBasket(int userId)
    {
        var result = await _basketService.CleanBasketAsync(userId);

        if (result.Contains("bulunamadı"))
            return NotFound(result);

        return Ok(result);
    }
}