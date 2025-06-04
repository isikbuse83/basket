using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ConsoleApp1.Infrastructure.Data
{
    public class DbFactory : IDesignTimeDbContextFactory<BasketDbContext>
    {
        public BasketDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BasketDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=LAPTOP-6PECIQ5E\\SQLEXPRESS;Database=BasketDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new BasketDbContext(optionsBuilder.Options);
        }
    }

}