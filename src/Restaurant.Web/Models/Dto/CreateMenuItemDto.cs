namespace Restaurant.Web.models.Dto;

public class CreateMenuItemDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
}