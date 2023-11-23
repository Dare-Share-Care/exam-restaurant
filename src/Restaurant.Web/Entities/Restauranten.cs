namespace Restaurant.Web.Entities;

public class Restauranten : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Zipcode { get; set; }
    public List<MenuItem> Menu { get; set; } = new(); 
}