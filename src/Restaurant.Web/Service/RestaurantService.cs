using Restaurant.Web.Entities;
using Restaurant.Web.Exceptions;
using Restaurant.Web.Interface.DomainServices;
using Restaurant.Web.Interface.Repositories;
using Restaurant.Web.models.Dto;
using Restaurant.Web.Specifications;

namespace Restaurant.Web.Service;

public class RestaurantService : IRestaurantService
{
    private readonly IReadRepository<Restauranten> _restaurantReadRepository;
    private readonly IRepository<Restauranten> _restaurantRepository;
    
    public RestaurantService(IReadRepository<Restauranten> restaurantReadRepository,
        IRepository<Restauranten> restaurantRepository)
    {
        _restaurantReadRepository = restaurantReadRepository;
        _restaurantRepository = restaurantRepository;
    }
    
     public async Task<RestaurantDto> CreateRestaurantAsync(string name)
    {
        var createdRestaurant = await _restaurantRepository.AddAsync(new Restauranten { Name = name });

        var restaurantDto = new RestaurantDto
        {
            Id = createdRestaurant.Id,
            Name = createdRestaurant.Name
        };
        return restaurantDto;
    }

    public async Task<List<MenuItemDto>> GetRestaurantMenuAsync(long restaurantId)
    {
        var restaurant =
            await _restaurantReadRepository.FirstOrDefaultAsync(new GetRestaurantWithMenuItemsSpec(restaurantId));
        if (restaurant is null)
        {
            throw new Exception($"Restaurant with id {restaurantId} not found");
        }

        var menu = restaurant.Menu.Select(menuItem => new MenuItemDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Price = menuItem.Price,
            Description = menuItem.Description
        }).ToList();

        return menu;
    }

    
    
    public async Task<RestaurantDto> AddMenuItemAsync(long restaurantId, CreateMenuItemDto dto)
    {
        //Get the restaurant we want to add the menu item to
        var restaurant = await _restaurantRepository.FirstOrDefaultAsync(new GetRestaurantWithMenuItemsSpec(restaurantId));

        if (restaurant is null)
        {
            throw new RestaurantException($"Restaurant with id {restaurantId} not found");
        }

        // Create a new menu item
        var menuItem = new MenuItem
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description
        };

        // Add the menu item to the restaurant
        restaurant.Menu.Add(menuItem);

        // Update the restaurant
        await _restaurantRepository.UpdateAsync(restaurant);

        // Return the updated restaurant to reflect the new menu
        var restaurantDto = new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Address = restaurant.Address,
            Zipcode = restaurant.Zipcode,
            Menu = restaurant.Menu.Select(item => new MenuItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description
            }).ToList()
        };

        return restaurantDto;
    }

    
    

    public async Task<RestaurantDto> RemoveMenuItemAsync(long restaurantId, long menuItemId)
    {
        //Get the restaurant we want to remove the menu item from
        var restaurant =
            await _restaurantRepository.FirstOrDefaultAsync(new GetRestaurantWithMenuItemsSpec(restaurantId));
        if (restaurant is null)
        {
            throw new RestaurantException($"Restaurant with id {restaurantId} not found");
        }

        //Remove the menu item from the restaurant
        var menuItem = restaurant.Menu.FirstOrDefault(x => x.Id == menuItemId);
        if (menuItem is null)
        {
            throw new RestaurantException($"Menu item with id {menuItemId} not found");
        }

        restaurant.Menu.Remove(menuItem);

        //Update the restaurant
        await _restaurantRepository.UpdateAsync(restaurant);


        //Return the updated restaurant to reflect the new menu
        var restaurantDto = new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Menu = restaurant.Menu.Select(item => new MenuItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            }).ToList()
        };

        return restaurantDto;
    }
    
    
    public async Task<List<RestaurantDto>> GetAllRestaurantsAsync()
    {
        var restaurants = await _restaurantReadRepository.ListAsync();
        var restaurantDtos = restaurants.Select(restaurant => new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Address = restaurant.Address,
            Zipcode = restaurant.Zipcode,
        }).ToList();

        return restaurantDtos;
    }
}