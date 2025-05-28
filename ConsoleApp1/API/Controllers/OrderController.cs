using System;
using System.Threading.Tasks;
using ConsoleApp1.Domain;
using ConsoleApp1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly BasketDb _db;

    public OrderController(BasketDb db)
    {
        _db = db;
    }

    [HttpPost("CompleteOrder/{userId}")]
    public async Task<IActionResult> CompleteOrder(int userId)
    {
        var basket = await _db.Basket
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.UserId == userId);

        if (basket == null || basket.Products.Count == 0)
            return BadRequest("Sepet boş veya bulunamadı.");

        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            foreach (var product in basket.Products)
            {
                if (!product.DecreaseDynamicStock())
                    return BadRequest($"Yetersiz stok: Ürün ID = {product.Id}");

                // Eğer WarehouseStock'u güncellemek istiyorsan, Product entity'sine uygun bir method eklemelisin.
            }

            basket.Products.Clear();

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok("Sipariş başarıyla oluşturuldu.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "Sipariş oluşturulurken hata oluştu: " + ex.Message);
        }
    }
}