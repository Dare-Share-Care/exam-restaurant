using Ardalis.Specification;
using Moq;
using Restaurant.Core.Exceptions;
using Restaurant.Core.Interfaces;
using Restaurant.Core.Models.Dto;
using Restaurant.Core.Services;
using Restaurant.Infrastructure.Entities;
using Restaurant.Infrastructure.Interfaces;

namespace Restaurant.Test.Tests.UnitTests
{
    public class RestaurantServiceUnitTests
    {
        private readonly IRestaurantService _restaurantService;
        private readonly Mock<IReadRepository<Restauranten>> _readRepoMock = new();
        private readonly Mock<IRepository<Restauranten>> _repoMock = new();

        public RestaurantServiceUnitTests()
        {
            _restaurantService = new RestaurantService(_readRepoMock.Object, _repoMock.Object, null);
        }



        [Fact]
        public async Task GetAllRestaurantsAsync_ShouldReturnAllRestaurants()
        {
            //Arrange
            var restaurantsResult = new List<Restauranten>
            {
                new() { Id = 1, Name = "Restaurant 1", Address = "Address 1", Zipcode = 1234 },
                new() { Id = 2, Name = "Restaurant 2", Address = "Address 2", Zipcode = 1234 },
                new() { Id = 3, Name = "Restaurant 3", Address = "Address 3", Zipcode = 1234 }
            };

            //Setup the mock to return the restaurants
            _readRepoMock.Setup(x => x.ListAsync(new CancellationToken()))
                .ReturnsAsync(restaurantsResult);

            //Act
            var restaurantsFound = await _restaurantService.GetAllRestaurantsAsync();

            //Assert
            //Contains 3 restaurants
            Assert.Equal(3, restaurantsFound.Count);

            //Contains the correct restaurants
            Assert.Equal("Restaurant 1", restaurantsFound[0].Name);
            Assert.Equal("Restaurant 2", restaurantsFound[1].Name);
            Assert.Equal("Restaurant 3", restaurantsFound[2].Name);
        }

        [Fact]
        public async Task CreateRestaurantAsync_ShouldCreateRestaurant()
        {
            //Arrange
            var dto = new CreateRestaurantDto{ Name = "Restaurant 1", Address = "Address 1", Zipcode = 1234};
            const string restaurantName = "Restaurant 1";
            var restaurant = new Restauranten { Id = 1, Name = restaurantName };

            //Setup the mock to return the restaurant
            _repoMock.Setup(x => x.AddAsync(It.IsAny<Restauranten>(), new CancellationToken()))
                .ReturnsAsync(restaurant);

            //Act
            var restaurantCreated = await _restaurantService.CreateRestaurantAsync(dto);

            //Assert
            Assert.Equal(restaurantName, restaurantCreated.Name);
        }

        [Fact]
        public async Task GetRestaurantMenuAsync_ShouldReturnMenu()
        {
            //Arrange
            var restaurantId = 1;
            var restaurant = new Restauranten
            {
                Id = restaurantId, Name = "Restaurant 1", Menu =
                {
                    new MenuItem { Id = 1, Name = "MenuItem 1", Price = 10 },
                    new MenuItem { Id = 2, Name = "MenuItem 2", Price = 20 },
                    new MenuItem { Id = 3, Name = "MenuItem 3", Price = 30 }
                }
            };

            //Setup the mock to return the restaurant
            _readRepoMock.Setup(x =>
                    x.FirstOrDefaultAsync(It.IsAny<ISpecification<Restauranten>>(), new CancellationToken()))
                .ReturnsAsync(restaurant);

            //Act
            var menu = await _restaurantService.GetRestaurantMenuAsync(restaurantId);

            //Assert

            //Contains 3 menu items
            Assert.Equal(3, menu.Count);

            //Contains the correct menu items
            Assert.Equal("MenuItem 1", menu[0].Name);
            Assert.Equal("MenuItem 2", menu[1].Name);
            Assert.Equal("MenuItem 3", menu[2].Name);
        }

        [Fact]
        public async Task AddMenuItemAsync_ShouldUpdateRestaurantWithNewMenuItem()
        {
            //Arrange
            const long restaurantId = 1;
            const string menuItemName = "MenuItem 1";
            const decimal menuItemPrice = 10;

            var restaurant = new Restauranten
            {
                Id = restaurantId, Name = "Restaurant 1", Menu =
                {
                    new MenuItem { Id = 1, Name = "MenuItem 1", Price = 10 },
                    new MenuItem { Id = 2, Name = "MenuItem 2", Price = 20 },
                    new MenuItem { Id = 3, Name = "MenuItem 3", Price = 30 }
                }
            };

            var createMenuItemDto = new CreateMenuItemDto() { Name = menuItemName, Price = menuItemPrice };

            //Setup the mock to return the restaurant
            _repoMock.Setup(x =>
                    x.FirstOrDefaultAsync(It.IsAny<ISpecification<Restauranten>>(), new CancellationToken()))
                .ReturnsAsync(restaurant);

            //Act
            var restaurantDto = await _restaurantService.AddMenuItemAsync(restaurantId, createMenuItemDto);

            //Assert

            //Contains the correct restaurant id
            Assert.Equal(restaurantId, restaurantDto.Id);

            // Contains 4 menu items
            Assert.Equal(4, restaurantDto.Menu.Count);

            // Contains the correct menu items
            Assert.Equal("MenuItem 1", restaurantDto.Menu[0].Name);
            Assert.Equal("MenuItem 2", restaurantDto.Menu[1].Name);
            Assert.Equal("MenuItem 3", restaurantDto.Menu[2].Name);
            Assert.Equal("MenuItem 1", restaurantDto.Menu[3].Name);
        }

       

        [Fact]
        public async Task RemoveMenuItemAsync_ShouldUpdateRestaurantWithRemovedMenuItem()
        {
            //Arrange
            const long restaurantId = 1;
            const long menuItemId = 1;

            var restaurant = new Restauranten
            {
                Id = restaurantId, Name = "Restaurant 1", Menu =
                {
                    new MenuItem { Id = 1, Name = "MenuItem 1", Price = 10 },
                    new MenuItem { Id = 2, Name = "MenuItem 2", Price = 20 },
                    new MenuItem { Id = 3, Name = "MenuItem 3", Price = 30 }
                }
            };

            //Setup the mock to return the restaurant
            _repoMock.Setup(x =>
                    x.FirstOrDefaultAsync(It.IsAny<ISpecification<Restauranten>>(), new CancellationToken()))
                .ReturnsAsync(restaurant);

            //Act
            var restaurantDto = await _restaurantService.RemoveMenuItemAsync(restaurantId, menuItemId);

            //Assert

            //Contains the correct restaurant id
            Assert.Equal(restaurantId, restaurantDto.Id);

            // Contains 2 menu items
            Assert.Equal(2, restaurantDto.Menu.Count);

            // Contains the correct menu items
            Assert.Equal("MenuItem 2", restaurantDto.Menu[0].Name);
            Assert.Equal("MenuItem 3", restaurantDto.Menu[1].Name);
        }

        [Fact]
        public async Task RemoveMenuItemAsync_ShouldThrowRestaurantException_WhenRestaurantNotFound()
        {
            //Arrange
            const long restaurantId = 1;
            const long menuItemId = 1;

            _repoMock.Setup(x =>
                    x.FirstOrDefaultAsync(It.IsAny<ISpecification<Restauranten>>(), new CancellationToken()))
                .ReturnsAsync((Restauranten)null);

            //Act & Assert
            await Assert.ThrowsAsync<RestaurantException>(() =>
                _restaurantService.RemoveMenuItemAsync(restaurantId, menuItemId));
        }


        [Fact]
        public async Task UpdateMenuItemAsync_ShouldUpdateMenuItem_WhenMenuItemExists()
        {
            // Arrange
            const long restaurantId = 1;
            const long menuItemId = 1;

            var restaurant = new Restauranten
            {
                Id = restaurantId,
                Name = "Restaurant 1",
                Menu =
                {
                    new MenuItem { Id = menuItemId, Name = "MenuItem 1", Price = 10, Description = "Original Description" },
                    new MenuItem { Id = 2, Name = "MenuItem 2", Price = 20 },
                    new MenuItem { Id = 3, Name = "MenuItem 3", Price = 30 }
                }
            };

            var updateMenuItemDto = new CreateMenuItemDto
            {
                Name = "Updated Item",
                Price = 12.99m,
                Description = "Updated Description"
            };

            _repoMock.Setup(x =>
                    x.FirstOrDefaultAsync(It.IsAny<ISpecification<Restauranten>>(), new CancellationToken()))
                .ReturnsAsync(restaurant);
            
            // Act
            var updatedMenuItem =
                await _restaurantService.UpdateMenuItemAsync(restaurantId, menuItemId, updateMenuItemDto);

            // Assert
            Assert.NotNull(updatedMenuItem);
            Assert.Equal(updateMenuItemDto.Name, updatedMenuItem.Name);
            Assert.Equal(updateMenuItemDto.Price, updatedMenuItem.Price);
            Assert.Equal(updateMenuItemDto.Description, updatedMenuItem.Description);

            // Check that the restaurant and menu item are updated in-memory
            Assert.Equal(updateMenuItemDto.Name, restaurant.Menu.First(x => x.Id == menuItemId).Name);
            Assert.Equal(updateMenuItemDto.Price, restaurant.Menu.First(x => x.Id == menuItemId).Price);
            Assert.Equal(updateMenuItemDto.Description, restaurant.Menu.First(x => x.Id == menuItemId).Description);
        }


    }
}