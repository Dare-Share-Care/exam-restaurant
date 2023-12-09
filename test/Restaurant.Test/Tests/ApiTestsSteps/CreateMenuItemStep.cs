using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Core.Models.Dto;
using Restaurant.Infrastructure.Data;
using Restaurant.Test.CustomFactories;
using Restaurant.Test.Helpers;
using TechTalk.SpecFlow;

namespace Restaurant.Test.Tests.ApiTestsSteps;

[Binding]
public class CreateMenuItemStep
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;

    public CreateMenuItemStep(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }


    [Given(@"a restaurant owner is logged into the system")]
    public void GivenARestaurantOwnerIsLoggedIntoTheSystem()
    {
        // Create a client to send requests to the test server representing the user
        _client = _factory.CreateClient();
        
        //Mock JWT token
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JwtTokenHelper.GetRestaurantOwnerJwtToken());
    }

    [Given(@"a restaurant already exists in the system")]
    public void GivenARestaurantAlreadyExistsInTheSystem()
    {
        //Populate the database with a restaurant
        var restaurant = MenuItemTestHelper.GetTestRestaurant();
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
        context.Restaurant.Add(restaurant);
        context.SaveChanges();
    }
    
    [When(@"the owner creates a new menu item,")]
    public async Task WhenTheOwnerCreatesANewMenuItem()
    {
        
        // DTO for the menu item to be added
        var createMenuItemDto = new CreateMenuItemDto()
        {
            Name = "New Menu Item", 
            Price = 12.99m, 
            Description = "Description for the new menu item"
        };
        var menuItemJson = JsonSerializer.Serialize(createMenuItemDto);
        var menuItemContent = new StringContent(menuItemJson, Encoding.UTF8, "application/json");

        _response = await _client!.PostAsync("/api/Restaurant/create-menuitem/1", menuItemContent);
    }

    [Then(@"the system should save the new menu in the database\.")]
    public async Task ThenTheSystemShouldSaveTheNewMenuInTheDatabase()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Check the in-memory database
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
        var createdMenuItem = await context.MenuItems.FirstOrDefaultAsync(u => u.Name == "New Menu Item");
        Assert.NotNull(createdMenuItem);
    }
}