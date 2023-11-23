using Microsoft.EntityFrameworkCore;
using Restaurant.Web.Entities;

namespace Restaurant.Web.Data;

public class RestaurantContext : DbContext
{
    public DbSet<Restauranten> Restaurant { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    
    public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary keys
        modelBuilder.Entity<Restauranten>().HasKey(r => r.Id);
        modelBuilder.Entity<MenuItem>().HasKey(mi => mi.Id);

        // Relationships
        modelBuilder.Entity<Restauranten>()
            .HasMany(r => r.Menu)
            .WithOne(mi => mi.Restauranten)
            .HasForeignKey(mi => mi.RestaurantId);
        
        
        //Decimal precision on price
        modelBuilder.Entity<MenuItem>()
            .Property(p => p.Price).HasColumnType("decimal(18,2)");
        
        // Set Seed Data
        modelBuilder.Entity<Restauranten>().HasData(
            new Restauranten{Id = 1, Name = "McDorra", Address = "Cool kid street", Zipcode = 42069}
            );

        // Set Seed Data
        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, RestaurantId = 1, Name = "Apple", Price = 1.99m, Description = "Green and from trees"},
            new MenuItem { Id = 2, RestaurantId = 1,Name = "Banana", Price = 2.99m, Description = "Green and from trees" },
            new MenuItem { Id = 3, RestaurantId = 1,Name = "Orange", Price = 3.99m, Description = "Green and from trees" }
        );
        
    }
}