using ConsoleApp1.Informations;
using Microsoft.EntityFrameworkCore;

public class BasketDb : DbContext
{
    public BasketDb(DbContextOptions<BasketDb> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>(); 
   public DbSet<User> Users => Set<User>();
}

