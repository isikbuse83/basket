using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DbContext = ConsoleApp1.Data.DbContext;

namespace ConsoleApp1.Services
{
    public class ProductService
    {
        private readonly DbContext _context;

        public ProductService(DbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            product.SetStock(product.WarehouseStock); 
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(int id, Product updated)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.ProductName = updated.ProductName;
            product.ProductDescription = updated.ProductDescription;
            product.ProductPrice = updated.ProductPrice;
            product.SetWarehouseStock(updated.WarehouseStock);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}