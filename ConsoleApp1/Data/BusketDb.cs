using ConsoleApp1.Informations;
using Microsoft.EntityFrameworkCore;

public class BusketDb : DbContext
{
    public BusketDb(DbContextOptions<BusketDb> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>(); 
   public DbSet<User> Users => Set<User>();
}

