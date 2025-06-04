using System;
using System.Threading.Tasks;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Application.Services
{
    public class OrderService
    {
        private readonly BasketDbContext _basketDbContext;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public OrderService(BasketDbContext basketDbContext, RabbitMQPublisher rabbitMQPublisher)
        {
            _basketDbContext = basketDbContext;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task<(bool Success, string Message)> CompleteOrderAsync(int userId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null || basket.Items.Count == 0)
                return (false, "Sepet boş veya bulunamadı.");

            using var transaction = await _basketDbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var basketItem in basket.Items)
                {
                    var product = basketItem.Product;

                    
                    for (int i = 0; i < basketItem.Quantity; i++)
                    {
                        if (!product.HasDynamicStock())
                            return (false, $"Yetersiz stok: Ürün ID = {product.Id}");
                    }

                    var order = new Order
                    {
                        ProductId = product.Id,
                        Quantity = basketItem.Quantity,
                        OrderDate = DateTime.UtcNow,
                        UserId = userId
                    };

                    _basketDbContext.Order.Add(order);
                    _rabbitMQPublisher.PublishOrder(order);
                }

                basket.ClearBasket();

                await _basketDbContext.SaveChangesAsync();
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
