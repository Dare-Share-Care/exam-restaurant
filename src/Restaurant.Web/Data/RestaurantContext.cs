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
        
     
    }
    
    
}