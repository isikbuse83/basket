using Microsoft.EntityFrameworkCore;
using ConsoleApp1.Domain;
using ConsoleApp1.Domain;

namespace ConsoleApp1.Data
{
    public class BasketDb : DbContext
    {
        public BasketDb(DbContextOptions<BasketDb> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>(); 
        public DbSet<User> Users => Set<User>();
        public DbSet<Basket> Basket { get; set; } 
    }
}
