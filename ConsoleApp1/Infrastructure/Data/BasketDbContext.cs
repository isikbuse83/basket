using ConsoleApp1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Infrastructure.Data
{
    public class BasketDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; } 
        public DbSet<User> Users { get; set; } 
        public DbSet<Basket> Basket { get; set; } 
        public DbSet<Order> Order { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>(b =>
            {
                b.OwnsMany(basket => basket.Items, a =>
                {
                    a.WithOwner().HasForeignKey("BasketId"); 
                    a.Property<int>("Id"); 
                    a.HasKey("Id");    
                });
            });
        }
    }
}
