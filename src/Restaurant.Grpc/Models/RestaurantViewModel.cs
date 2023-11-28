namespace Restaurant.Grpc.Models;

public class RestaurantViewModel
{
    public long Id { get; set; }
    
    public List<MenuItemViewModel> Menu { get; set; } = new();
}