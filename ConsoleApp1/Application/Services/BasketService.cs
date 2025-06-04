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
                .AsSplitQuery() //performans
                .Include(b => b.Items)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            return basket == null ? null : _mapper.Map<BasketResponse>(basket);
        }

        public async Task<string> AddToBasketAsync(int userId, int productId)
        {
            var product = await _basketDbContext.Products.FindAsync(productId);
            
            if (product == null)
                return "Ürün bulunamadı";

            if (!product.HasDynamicStock())
                return "Yetersiz stok";
            
            var basket = await GetBasketByUserId(userId);

            var basketItem = GetBasketItem(productId, basket);
            basketItem.IncreaseQuantity();

            product.DecreaseDynamicStock();

            await _basketDbContext.SaveChangesAsync();

            return "Ürün sepete eklendi";
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

        public async Task<string> RemoveFromBasketAsync(int userId, int productId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
                return "Sepet bulunamadı";

            var basketItem = basket.Items.FirstOrDefault(bi => bi.ProductId == productId);
            if (basketItem == null)
                return "Ürün sepette bulunamadı";

            basket.RemoveBasketItem(basketItem);

            var product = await _basketDbContext.Products.FindAsync(productId);
            product?.IncreaseDynamicStock();

            await _basketDbContext.SaveChangesAsync();

            return "Sepetten ürün çıkarıldı.";
        }

        public async Task<string> CleanBasketAsync(int userId)
        {
            var basket = await _basketDbContext.Basket
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
                return "Sepet bulunamadı";

            foreach (var basketItem in basket.Items)
            {
                var product = await _basketDbContext.Products.FindAsync(basketItem.ProductId);
                product?.IncreaseDynamicStock();
            }

            basket.ClearBasket();
            
            await _basketDbContext.SaveChangesAsync();

            return "Sepetiniz boşaltıldı.";
        }
    }
}