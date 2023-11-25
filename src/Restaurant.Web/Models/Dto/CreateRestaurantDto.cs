namespace Restaurant.Web.models.Dto;

public class CreateRestaurantDto 
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int Zipcode { get; set; }
}