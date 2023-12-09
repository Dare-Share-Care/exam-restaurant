using Restaurant.Core.Models.Dto;

namespace Restaurant.Core.Models.ViewModels;

public class RestaurantMenuViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int Zipcode { get; set; }
    
    public List<MenuItemDto> Menu { get; set; } = new(); 
}