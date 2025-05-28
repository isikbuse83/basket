using System.Threading.Tasks;
using ConsoleApp1.Domain;
using ConsoleApp1.Data;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Controllers;


[ApiController]
[Route("api/[controller]")]

public class BasketsController: ControllerBase
{
    private readonly BasketDb _db;
    
    public BasketsController(BasketDb db)
    {
        _db = db;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult> GetBasket(int userId)
    {
        var basket = await _db.Basket
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.UserId == userId);

        if (basket == null)
            return NotFound("Sepet bulunamadı");

        return Ok(basket);
    }

    [HttpPost("{userId}/add/{productId}")]
    public async Task<ActionResult> AddBasket(int userId, int productId)
    {
        var basket = await _db.Basket
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.UserId == userId);

        var product = await _db.Products.FindAsync(productId);
        
        if (basket == null || product == null)
        {
            return NotFound("Sepet veya ürün bulunamadı");
        }

        if (!product.DecreaseDynamicStock())
        {
            return BadRequest("Yetersiz stok");
        }

        basket.Products.Add(product);
        await _db.SaveChangesAsync();

        return Ok("Ürün sepete eklendi");
    }
    
    [HttpDelete("{userId}/remove/{productId}")]
    public async Task<ActionResult> RemoveBasket(int userId, int productId)
    {
        var basket = await _db.Basket
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.UserId == userId);

        if (basket == null)
        {
            return NotFound("Sepet bulunamadı");
        }

        var product = await _db.Products.FindAsync(productId);

        if (product == null || !basket.Products.Contains(product))
        {
            return NotFound("Ürün sepette bulunamadı");
        }

        basket.Products.Remove(product);
        product.IncreaseDynamicStock();

        await _db.SaveChangesAsync();

        return Ok("Sepetten ürün çıkarıldı.");
    }

    [HttpDelete("{userId}/clean")]
    public async Task<ActionResult> CleanBasket(int userId)
    {
        var basket = await _db.Basket
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.UserId == userId);

        if (basket == null)
        {
            return NotFound("Sepet bulunamadı");
        }

        foreach (var product in basket.Products)
        {
            product.IncreaseDynamicStock();
        }

        basket.Products.Clear();
        await _db.SaveChangesAsync();

        return Ok("Sepetiniz boşaltıldı.");
    }
}

 