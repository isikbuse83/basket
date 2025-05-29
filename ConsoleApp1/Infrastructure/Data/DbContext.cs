using Microsoft.EntityFrameworkCore;
using ConsoleApp1.Domain;
using ConsoleApp1.Domain.Entities;

namespace ConsoleApp1.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>(); 
        public DbSet<User> Users => Set<User>();
        public DbSet<Basket> Basket { get; set; } 
        
        public DbSet<Order> Order { get; set; }
    }
}
