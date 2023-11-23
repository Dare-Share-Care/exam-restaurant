namespace Restaurant.Web.Entities;

public class MenuItem : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    
    public long RestaurantId { get; set; } 
    public Restauranten Restauranten { get; set; } = null!;
}