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


    internal static List<MenuItem> CreateTestMenuItems()
    {
        var menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Id = 1,
                Name = "Test Menu Item 1",
                Price = 12.99m,
                Description = "Test Description 1",
                RestaurantId = 1
            },
            new MenuItem
            {
                Id = 2,
                Name = "Test Menu Item 2",
                Price = 15.99m,
                Description = "Test Description 2",
                RestaurantId = 1
            },
            new MenuItem
            {
                Id = 3,
                Name = "Test Menu Item 3",
                Price = 18.99m,
                Description = "Test Description 3",
                RestaurantId = 1
            },
            new MenuItem
            {
                Id = 4,
                Name = "Test Menu Item 4",
                Price = 21.99m,
                Description = "Test Description 4",
                RestaurantId = 1
            },
        };

        return menuItems;
    }

    
    
    
    
    
}