using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Test.CustomFactories;
using Restaurant.Web.Data;
using Restaurant.Web.Entities;
using Restaurant.Web.models.Dto;
using System.Text.Json;

namespace Restaurant.Test;

public class RestaurantControllerApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public RestaurantControllerApiTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
   
    [Fact]
    public async Task GetAllRestaurantEndpoint_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/restaurant/get-all-restaurants");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
       
    }
    
    
    
  [Fact]
  public async Task AddMenuItemEndpoint_ReturnsSuccessStatusCode()
{
    // Arrange
    var client = _factory.CreateClient();
    
    
    // Add a restaurant to the database (use your actual restaurant data)
    var restaurantId = 1;
    var restaurant = new Restauranten { Id = restaurantId, Name = "Test Restaurant", Address = "Test Address", Zipcode = 12345 };
    var restaurantJson = JsonSerializer.Serialize(restaurant);
    var restaurantContent = new StringContent(restaurantJson, Encoding.UTF8, "application/json");
    
    var restaurantResponse = await client.PostAsync("/api/restaurant/create-restaurant", restaurantContent);
    restaurantResponse.EnsureSuccessStatusCode();
    
    // DTO for the menu item to be added
    var menuItemDto = new MenuItemDto
    {
        Name = "New Menu Item", 
        Price = 12.99m, 
        Description = "Description for the new menu item"
    };
    var menuItemJson = JsonSerializer.Serialize(menuItemDto);
    var menuItemContent = new StringContent(menuItemJson, Encoding.UTF8, "application/json");

    // Act
    var response = await client.PostAsync($"/api/restaurant/add-menuitem/{restaurantId}", menuItemContent);

    // Assert
    response.EnsureSuccessStatusCode(); // Status Code 200-299
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    
    // Check the in-memory database or actual database
    using var scope = _factory.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
    
    // Retrieve the restaurant with menu items
    var updatedRestaurant = await context.Restaurant
        .Include(r => r.Menu)
        .FirstOrDefaultAsync(r => r.Id == restaurantId);

    // Assert the restaurant and menu item
    Assert.NotNull(updatedRestaurant);
    Assert.Single(updatedRestaurant.Menu); // Assuming only one menu item is added in this test
    
    var addedMenuItem = updatedRestaurant.Menu.First();
    Assert.Equal(menuItemDto.Name, addedMenuItem.Name);
    Assert.Equal(menuItemDto.Price, addedMenuItem.Price);
    Assert.Equal(menuItemDto.Description, addedMenuItem.Description);
}

  
  
 [Fact]
public async Task RemoveMenuItemEndpoint_ReturnsSuccessStatusCode()
{
    // Arrange
    var client = _factory.CreateClient();

    // Add a restaurant to the database
    var restaurantId = 1;
    var menuItemId = 1;
    var restaurant = new Restauranten { Id = restaurantId, Name = "Test Restaurant", Address = "Test Address", Zipcode = 12345 };
    var restaurantJson = JsonSerializer.Serialize(restaurant);
    var restaurantContent = new StringContent(restaurantJson, Encoding.UTF8, "application/json");

    var restaurantResponse = await client.PostAsync("/api/restaurant/create-restaurant", restaurantContent);
    restaurantResponse.EnsureSuccessStatusCode();

    // Add a menu item to the restaurant
    var menuItemDto = new MenuItemDto
    {
        Name = "New Menu Item",
        Price = 12.99m,
        Description = "Description for the new menu item"
    };
    var menuItemJson = JsonSerializer.Serialize(menuItemDto);
    var menuItemContent = new StringContent(menuItemJson, Encoding.UTF8, "application/json");

    var addMenuItemResponse = await client.PostAsync($"/api/restaurant/add-menuitem/{restaurantId}", menuItemContent);
    addMenuItemResponse.EnsureSuccessStatusCode();

    // Act
    var removeMenuItemResponse = await client.DeleteAsync($"/api/restaurant/remove-menuitem/{restaurantId}?menuItemId={menuItemId}");

    // Assert
    removeMenuItemResponse.EnsureSuccessStatusCode(); // Status Code 200-299
    Assert.Equal(HttpStatusCode.OK, removeMenuItemResponse.StatusCode);

    // Check the in-memory database or actual database
    using var scope = _factory.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();

    // Retrieve the restaurant after removing the menu item
    var updatedRestaurant = await context.Restaurant
        .Include(r => r.Menu)
        .FirstOrDefaultAsync(r => r.Id == restaurantId);

    // Assert that the restaurant exists and has no menu items
    Assert.NotNull(updatedRestaurant);
    Assert.Empty(updatedRestaurant.Menu);
}


}
