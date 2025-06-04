using ConsoleApp1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Infrastructure.Data
{
    public class BasketDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<Order> Order { get; set; }


        //Fluent API 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Basket ilişkileri
            modelBuilder.Entity<Basket>(b =>
            {
                b.OwnsMany(basket => basket.Items, a =>
                {
                    a.WithOwner().HasForeignKey("BasketId");
                    a.Property<int>("Id");
                    a.HasKey("Id");
                });
            });

            // Order → Product ilişkisi
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany() // Order -> Product: Many orders can have one product
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order → User ilişkisi
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany() // One user can have many orders
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
