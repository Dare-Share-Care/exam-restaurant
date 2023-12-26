using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Restaurant.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RestaurantContext>
{
    public RestaurantContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RestaurantContext>();
        optionsBuilder.UseSqlServer("Server=mssql;Database=Restaurant;User Id=sa;Password=thisIsSuperStrong1234;TrustServerCertificate=True");

        return new RestaurantContext(optionsBuilder.Options);
    }
}