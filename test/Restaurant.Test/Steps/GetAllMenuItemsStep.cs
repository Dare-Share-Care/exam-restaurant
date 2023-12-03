using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Infrastructure.Data;
using Restaurant.Test.CustomFactories;
using Restaurant.Test.Helpers;
using TechTalk.SpecFlow;

namespace Restaurant.Test.Steps;

[Binding]
public class GetAllMenuItemsStep
{
    
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;
    
    
    public GetAllMenuItemsStep(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Given(@"A restaurant exists in the system")]
    public void GivenARestaurantAlreadyExistsInTheSystem()
    {
        // Clear existing restaurants
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
        context.Restaurant.RemoveRange(context.Restaurant);
        context.SaveChanges();

        // Add a new restaurant
        var restaurant = MenuItemTestHelper.GetTestRestaurant();
        context.Restaurant.Add(restaurant);
        context.SaveChanges();
    }
    
    [Given(@"Menu item exists in the system,")]
    public void GivenMenuItemsHasBeenCreated()
    {
        var menuItems = MenuItemTestHelper.CreateTestMenuItems();
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();    
        context.MenuItems.AddRange(menuItems);
        context.SaveChanges();
    }

    [When(@"the user accesses a given restaurant,")]
    public async Task WhenTheUserAccessesTheAGivenRestaurant()
    {
        var restaurantId = 1; 
        var requestUri = "/api/restaurant/1/menu"; 
        _client = _factory.CreateClient();
        _response = await _client.GetAsync(requestUri);
    }

    [Then(@"they should see the list of all existing menu items\.")]
    public async Task ThenTheyShouldSeeTheListOfAllExistingMenuItems()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Check the in-memory database
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
    
        // Retrieve all menu items from the database
        var allMenuItems = await context.MenuItems.ToListAsync();
    
        // Assert that there are menu items in the database
        allMenuItems.Should().NotBeNull();
        allMenuItems.Should().HaveCountGreaterThan(0);

        // You can further assert specific properties of menu items if needed
        allMenuItems.Should().Contain(menuItem => menuItem.Name == "Test Menu Item 1");
        allMenuItems.Should().Contain(menuItem => menuItem.Name == "Test Menu Item 2");
        // Add more assertions as needed
    }

}