using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.DTOs.Response;
using ConsoleApp1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Application.Services
{
    public class BasketService
    {
        private readonly BasketDbContext _basketDbContext;
        private readonly IMapper _mapper;

        public BasketService(BasketDbContext basketDbContext, IMapper mapper)
        {
            _basketDbContext = basketDbContext;
            _mapper = mapper;
        }
        
        public async Task<BasketResponse> GetBasketAsync(int userId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)  // sadece Items, Product değil
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null) throw new InvalidOperationException("Sepet bulunamdı.");

            // Sepetteki ürün id'lerini al
            var productIds = basket.Items.Select(i => i.ProductId).ToList();

            // Ürünleri ayrı sorgu ile çek
            var products = await _basketDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // Product nesnelerini basket itemlara manuel set et
            foreach (var item in basket.Items)
            {
                item.Product = products.FirstOrDefault(p => p.Id == item.ProductId);
            }

            return _mapper.Map<BasketResponse>(basket);
        }


        public async Task AddToBasketAsync(int userId, int productId)
        {
            var product = await _basketDbContext.Products.FindAsync(productId);
            
            if (product == null)
                throw new InvalidDataException("Ürün bulunamadı.");

            if (!product.HasDynamicStock()) throw new InvalidOperationException("Ürün bulunamadı.");
            
            var basket = await GetBasketByUserId(userId);

            var basketItem = GetBasketItem(productId, basket);
            basketItem.IncreaseQuantity();

            product.DecreaseDynamicStock();

            await _basketDbContext.SaveChangesAsync();
        }

        private static BasketItem GetBasketItem(int productId, Basket basket)
        {
            var basketItem = basket.Items.FirstOrDefault(bi => bi.ProductId == productId);

            if (basketItem != null)
            {
                return basketItem;
            }
            
            var newBasketItem = new BasketItem(productId);
            basket.AddBasketItem(newBasketItem);
            
            return newBasketItem;
        }

        private async Task<Basket> GetBasketByUserId(int userId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket != null) return basket;
            
            basket = new Basket(userId);
            await _basketDbContext.Basket.AddAsync(basket);

            return basket;
        }

        public async Task RemoveFromBasketAsync(int userId, int productId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
                throw new InvalidOperationException("Sepet bulunamadı");

            var basketItem = basket.Items.FirstOrDefault(bi => bi.ProductId == productId);
            if (basketItem == null)
                throw new InvalidOperationException("Silinmek istenen ürün bulunamadı");

            basket.RemoveBasketItem(basketItem);

            var product = await _basketDbContext.Products.FindAsync(productId);
            product?.IncreaseDynamicStock();

            await _basketDbContext.SaveChangesAsync();
        }

        public async Task CleanBasketAsync(int userId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null) throw new InvalidOperationException("Sepet bulunamadı");

            foreach (var basketItem in basket.Items)
            {
                var product = await _basketDbContext.Products.FindAsync(basketItem.ProductId);
                product?.IncreaseDynamicStock();
            }

            basket.ClearBasket();
            
            await _basketDbContext.SaveChangesAsync();
        }
    }
}