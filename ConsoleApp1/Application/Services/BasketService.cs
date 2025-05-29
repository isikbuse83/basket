using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.Reponse;
using Microsoft.EntityFrameworkCore;
using DbContext = ConsoleApp1.Data.DbContext;

namespace ConsoleApp1.Services
{
    public class BasketService
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public BasketService(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Artık domain Basket değil BasketResponseDto dönecek
        public async Task<BasketResponse> GetBasketAsync(int userId)
        {
            var basket = await _dbContext.Basket
                .Include(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
                return null;

            return _mapper.Map<BasketResponse>(basket);
        }

        public async Task<string> AddToBasketAsync(int userId, int productId)
        {
            var basket = await _dbContext.Basket
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
            {
                
                basket = new Basket { UserId = userId };
                await _dbContext.Basket.AddAsync(basket);
                await _dbContext.SaveChangesAsync();
            }

            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
                return "Ürün bulunamadı";

            if (!product.DecreaseDynamicStock())
                return "Yetersiz stok";

            var basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
            if (basketItem != null)
            {
                basketItem.Quantity++;
            }
            else
            {
                basket.BasketItems.Add(new BasketItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    Basket = basket
                });
            }

            await _dbContext.SaveChangesAsync();

            return "Ürün sepete eklendi";
        }

        public async Task<string> RemoveFromBasketAsync(int userId, int productId)
        {
            var basket = await _dbContext.Basket
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
                return "Sepet bulunamadı";

            var basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
            if (basketItem == null)
                return "Ürün sepette bulunamadı";

            basket.BasketItems.Remove(basketItem);

            var product = await _dbContext.Products.FindAsync(productId);
            product?.IncreaseDynamicStock();

            await _dbContext.SaveChangesAsync();

            return "Sepetten ürün çıkarıldı.";
        }

        public async Task<string> CleanBasketAsync(int userId)
        {
            var basket = await _dbContext.Basket
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
                return "Sepet bulunamadı";

            foreach (var basketItem in basket.BasketItems)
            {
                var product = await _dbContext.Products.FindAsync(basketItem.ProductId);
                product?.IncreaseDynamicStock();
            }

            basket.BasketItems.Clear();
            await _dbContext.SaveChangesAsync();

            return "Sepetiniz boşaltıldı.";
        }
    }
}

