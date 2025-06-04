using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ConsoleApp1.DTOs.Response;

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

        // Siparişi tamamlama metodu - JSON DTO döner
        public async Task<CompleteOrderResponse> CompleteOrderAsync(int userId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null || basket.Items == null || basket.Items.Count == 0)
            {
                return new CompleteOrderResponse
                {
                    Message = "Sepet boş veya bulunamadı.",
                    Orders = new List<OrderResult>()
                };
            }

            using var transaction = await _basketDbContext.Database.BeginTransactionAsync();

            try
            {
                var orderResults = new List<OrderResult>();

                foreach (var basketItem in basket.Items)
                {
                    var product = await _basketDbContext.Products.FindAsync(basketItem.ProductId);
                    if (product == null || !product.HasDynamicStock())
                    {
                        return new CompleteOrderResponse
                        {
                            Message = $"Ürün bulunamadı veya yetersiz stok: ID = {basketItem.ProductId}",
                            Orders = new List<OrderResult>()
                        };
                    }

                    for (int i = 0; i < basketItem.Quantity; i++)
                        product.DecreaseDynamicStock();

                    var order = new Order
                    {
                        ProductId = product.Id,
                        Quantity = basketItem.Quantity,
                        OrderDate = DateTime.UtcNow,
                        UserId = userId
                    };

                    _basketDbContext.Order.Add(order);
                    await _basketDbContext.SaveChangesAsync(); 
                    _rabbitMQPublisher.PublishOrder(order);

                    orderResults.Add(new OrderResult
                    {
                        OrderId = order.Id,
                        ProductName = product.ProductName, 
                        Price = product.ProductPrice,       
                        Quantity = basketItem.Quantity
                    });
                }

                basket.ClearBasket();
                await _basketDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CompleteOrderResponse
                {
                    Message = "Sipariş başarıyla oluşturuldu.",
                    Orders = orderResults
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new CompleteOrderResponse
                {
                    Message = $"Sipariş oluşturulurken hata oluştu: {ex.Message}",
                    Orders = new List<OrderResult>()
                };
            }
        }

        // Siparişi ID'ye göre getir
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _basketDbContext.Order
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
