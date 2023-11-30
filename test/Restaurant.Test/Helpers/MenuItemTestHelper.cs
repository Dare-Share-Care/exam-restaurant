using Restaurant.Infrastructure.Entities;

namespace Restaurant.Test.Helpers;

internal static class MenuItemTestHelper
{
    internal static Restauranten GetTestRestaurant()
    {
        var restaurant = new Restauranten
        {
            Id = 1,
            Name = "Test Restaurant",
            Address = "Test Address",
            Zipcode = 12345
        };
        return restaurant;
    }
}