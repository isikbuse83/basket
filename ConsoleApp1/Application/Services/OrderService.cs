using System;
using System.Threading.Tasks;
using ConsoleApp1.Data;
using ConsoleApp1.Domain;
using ConsoleApp1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ConsoleApp1.Infrastructure.Services;
using DbContext = ConsoleApp1.Data.DbContext;

namespace ConsoleApp1.Services
{
    public class OrderService
    {
        private readonly DbContext _dbContext;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public OrderService(DbContext dbContext, RabbitMQPublisher rabbitMQPublisher)
        {
            _dbContext = dbContext;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task<(bool Success, string Message)> CompleteOrderAsync(int userId)
        {
            var basket = await _dbContext.Basket
                .Include(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null || basket.BasketItems.Count == 0)
                return (false, "Sepet boş veya bulunamadı.");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var basketItem in basket.BasketItems)
                {
                    var product = basketItem.Product;

                    
                    for (int i = 0; i < basketItem.Quantity; i++)
                    {
                        if (!product.DecreaseDynamicStock())
                            return (false, $"Yetersiz stok: Ürün ID = {product.Id}");
                    }

                    var order = new Order
                    {
                        ProductId = product.Id,
                        Quantity = basketItem.Quantity,
                        OrderDate = DateTime.UtcNow,
                        UserId = userId
                    };

                    _dbContext.Order.Add(order);
                    _rabbitMQPublisher.PublishOrder(order);
                }

                basket.BasketItems.Clear();

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Sipariş başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, "Sipariş oluşturulurken hata oluştu: " + ex.Message);
            }
        }

        public async Task<object> GetOrderByIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
