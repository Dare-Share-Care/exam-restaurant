using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Core.Interfaces;
using Restaurant.Core.Models.Dto;
using Restaurant.Core.Services;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Entities;
using Restaurant.Infrastructure.Interfaces;

namespace Restaurant.Test.Tests.IntegrationTests
{
    public class RestaurantServiceIntegrationTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly RestaurantContext _dbContext;

        public RestaurantServiceIntegrationTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddDbContext<RestaurantContext>(options => options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()))
                .AddScoped<IRepository<Restauranten>, EfRepository<Restauranten>>()
                .AddScoped<IRestaurantService, RestaurantService>()
                .AddScoped<IReadRepository<Restauranten>, EfRepository<Restauranten>>()
                .AddScoped<ILoggingService, LoggingService>()
                .BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<RestaurantContext>();
        }
  
        [Fact]
        public async Task GetAllRestaurantsAsync_ShouldReturnAllRestaurants()
        {
            // Arrange
            var restaurantService = _serviceProvider.GetRequiredService<IRestaurantService>();

            // Act
            var restaurantsFound = await restaurantService.GetAllRestaurantsAsync();

            // Assert
            Assert.Equal(0, restaurantsFound.Count); // In-memory database is initially empty

            // Add some test data to the in-memory database
            _dbContext.Restaurant.AddRange(new[]
            {
                new Restauranten { Id = 1, Name = "Restaurant 1", Address = "Address 1", Zipcode = 1234 },
                new Restauranten { Id = 2, Name = "Restaurant 2", Address = "Address 2", Zipcode = 1234 },
                new Restauranten { Id = 3, Name = "Restaurant 3", Address = "Address 3", Zipcode = 1234 }
            });
            await _dbContext.SaveChangesAsync();

            // Act again after adding data
            restaurantsFound = await restaurantService.GetAllRestaurantsAsync();

            // Assert
            Assert.Equal(3, restaurantsFound.Count);
            Assert.Equal("Restaurant 1", restaurantsFound[0].Name);
            Assert.Equal("Restaurant 2", restaurantsFound[1].Name);
            Assert.Equal("Restaurant 3", restaurantsFound[2].Name);
        }

        
        [Fact]
        public async Task GetRestaurantMenuAsync_ShouldReturnMenu()
        {
            // Arrange
            var restaurantService = _serviceProvider.GetRequiredService<IRestaurantService>();

            // Add a restaurant with a menu to the in-memory database
            var restaurantId = 1;
            var restaurant = new Restauranten
            {
                Id = restaurantId,
                Name = "Test Restaurant",
                Address = "Test Address",  // Set the address property
                Zipcode = 12345,
                Menu = new List<MenuItem>
                {
                    new MenuItem { Id = 1, Name = "Menu Item 1", Price = 10.99m, Description = "Description 1" },
                    new MenuItem { Id = 2, Name = "Menu Item 2", Price = 15.99m, Description = "Description 2" },
                    new MenuItem { Id = 3, Name = "Menu Item 3", Price = 8.99m, Description = "Description 3" }
                }
            };
            _dbContext.Restaurant.Add(restaurant);
            await _dbContext.SaveChangesAsync();

            // Act
            var menu = await restaurantService.GetRestaurantMenuAsync(restaurantId);

            // Assert
            Assert.NotNull(menu);
            Assert.Equal(3, menu.Count);

            // Check if menu items are correctly returned
            Assert.Equal("Menu Item 1", menu[0].Name);
            Assert.Equal(10.99m, menu[0].Price);
            Assert.Equal("Description 1", menu[0].Description);

            Assert.Equal("Menu Item 2", menu[1].Name);
            Assert.Equal(15.99m, menu[1].Price);
            Assert.Equal("Description 2", menu[1].Description);

            Assert.Equal("Menu Item 3", menu[2].Name);
            Assert.Equal(8.99m, menu[2].Price);
            Assert.Equal("Description 3", menu[2].Description);
        }

        [Fact]
        public async Task GetRestaurantMenuAsync_ShouldThrowException_WhenRestaurantNotFound()
        {
            // Arrange
            var restaurantService = _serviceProvider.GetRequiredService<IRestaurantService>();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => restaurantService.GetRestaurantMenuAsync(999));
        }
        
        
        [Fact]
        public async Task AddMenuItemAsync_ShouldAddMenuItemToRestaurant()
        {
            // Arrange
            var restaurantService = _serviceProvider.GetRequiredService<IRestaurantService>();

            // Add a restaurant to the in-memory database
            var restaurantId = 1;
            var restaurant = new Restauranten
            {
                Id = restaurantId,
                Name = "Test Restaurant",
                Address = "Test Address",
                Zipcode = 12345
            };
            _dbContext.Restaurant.Add(restaurant);
            await _dbContext.SaveChangesAsync();

            // DTO for the menu item to be added
            var menuItemDto = new CreateMenuItemDto
            {
                Name = "New Menu Item",
                Price = 12.99m,
                Description = "Description for the new menu item"
            };
 
            // Act
            var updatedRestaurantDto = await restaurantService.AddMenuItemAsync(restaurantId, menuItemDto);

            // Assert
            Assert.NotNull(updatedRestaurantDto);
            Assert.Equal(1, updatedRestaurantDto.Menu.Count);

            var addedMenuItem = updatedRestaurantDto.Menu.First();
            Assert.Equal(menuItemDto.Name, addedMenuItem.Name);
            Assert.Equal(menuItemDto.Price, addedMenuItem.Price);
            Assert.Equal(menuItemDto.Description, addedMenuItem.Description);
        }
        
        
        [Fact]
        public async Task RemoveMenuItemAsync_ShouldRemoveMenuItemFromRestaurant()
        {
            // Arrange
            var restaurantService = _serviceProvider.GetRequiredService<IRestaurantService>();

            // Add a restaurant to the in-memory database with a menu item
            var restaurantId = 1;
            var menuItemId = 1;
            var restaurant = new Restauranten
            {
                Id = restaurantId,
                Name = "Test Restaurant",
                Address = "Test Address",
                Zipcode = 12345,
                Menu = { new MenuItem { Id = menuItemId, Name = "Test Menu Item", Price = 9.99m, Description = "Test Description" } }
            };
            _dbContext.Restaurant.Add(restaurant);
            await _dbContext.SaveChangesAsync();

            // Act
            var updatedRestaurantDto = await restaurantService.RemoveMenuItemAsync(restaurantId, menuItemId);

            // Assert
            Assert.NotNull(updatedRestaurantDto);
            Assert.Empty(updatedRestaurantDto.Menu);

            // Additional assertion to check if the relationship is properly updated
            var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == menuItemId);
            Assert.Null(menuItem); // Ensure that the menu item is removed from the database

            // Check if the restaurant's menu is also updated in the database
            var restaurantInDb = await _dbContext.Restaurant.Include(r => r.Menu).FirstOrDefaultAsync(r => r.Id == restaurantId);
            Assert.Empty(restaurantInDb.Menu);
        }
        
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
