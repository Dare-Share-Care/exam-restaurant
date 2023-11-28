namespace Restaurant.Core.Models.Dto;

public class RestaurantDto : BaseDto
{
    
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Zipcode { get; set; }
    public List<MenuItemDto> Menu { get; set; } = new(); 
}