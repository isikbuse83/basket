using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ConsoleApp1.Data;

namespace ConsoleApp1.Data
{
    public class BasketDbFactory : IDesignTimeDbContextFactory<BasketDb>
    {
        public BasketDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BasketDb>();
            optionsBuilder.UseSqlServer(
                "Server=LAPTOP-6PECIQ5E\\SQLEXPRESS;Database=Basket;Trusted_Connection=True;TrustServerCertificate=True;");

            return new BasketDb(optionsBuilder.Options);
        }
    }

}