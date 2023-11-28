namespace Restaurant.Core.Models.Dto;

public class MenuItemDto : BaseDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    
    
}