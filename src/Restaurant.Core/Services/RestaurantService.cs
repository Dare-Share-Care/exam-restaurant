using Microsoft.Extensions.Logging;
using Restaurant.Core.Interfaces;
using Restaurant.Core.Models.Dto;
using Restaurant.Core.Models.ViewModels;
using Restaurant.Core.Exceptions;
using Restaurant.Infrastructure.Entities;
using Restaurant.Infrastructure.Interfaces;
using Restaurant.Infrastructure.Specifications;

namespace Restaurant.Core.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IReadRepository<Restauranten> _restaurantReadRepository;
    private readonly IRepository<Restauranten> _restaurantRepository;
    private readonly ILoggingService _logger;

    public RestaurantService(IReadRepository<Restauranten> restaurantReadRepository, IRepository<Restauranten> restaurantRepository, ILoggingService logger)
    {
        _restaurantReadRepository = restaurantReadRepository;
        _restaurantRepository = restaurantRepository;
        _logger = logger;
    }
  

    public async Task<RestaurantViewModel> CreateRestaurantAsync(CreateRestaurantDto dto)
    {
        try
        {
            var createdRestaurant = await _restaurantRepository.AddAsync(new Restauranten
            {
                Name = dto.Name,
                Address = dto.Address,
                Zipcode = dto.Zipcode
            });

            var restaurantViewModel = new RestaurantViewModel()
            {
                Id = createdRestaurant.Id,
                Name = createdRestaurant.Name,
                Address = createdRestaurant.Address,
                Zipcode = createdRestaurant.Zipcode
            };
            return restaurantViewModel;
        }
        catch (Exception ex)
        {
            await _logger.LogToFile(LogLevel.Error, "Something went wrong trying to create a restaurant.", ex);
            throw;
        }
       
    }


    public async Task<List<MenuItemDto>> GetRestaurantMenuAsync(long restaurantId)
    {
        try
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
        catch (Exception ex)
        {
            await _logger.LogToFile(LogLevel.Error, "Something went wrong trying to get the menu for a restaurant.", ex);
            throw;
        }
    }



    public async Task<RestaurantDto> AddMenuItemAsync(long restaurantId, CreateMenuItemDto dto)
    {
        try
        {
            //Get the restaurant we want to add the menu item to
            var restaurant =
                await _restaurantRepository.FirstOrDefaultAsync(new GetRestaurantWithMenuItemsSpec(restaurantId));

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
        catch (Exception ex)
        {
            await _logger.LogToFile(LogLevel.Error, "Something went wrong trying to add a menu item to a restaurant.", ex);
            throw;
        }
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


    public async Task<List<RestaurantViewModel>> GetAllRestaurantsAsync()
    {
        try
        {
            var restaurants = await _restaurantReadRepository.ListAsync();
            var restaurantViewModels = restaurants.Select(restaurant => new RestaurantViewModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                Zipcode = restaurant.Zipcode,
            }).ToList();


            return restaurantViewModels;
        }
        catch (Exception ex)
        {
            // Log an error with an exception
            await _logger.LogToFile(LogLevel.Error, "Something went wrong.", ex);
            throw;
        }
    }

    public async Task<RestaurantDto> GetRestaurantById(long restaurantId)
    {
        try
        {
            var order =  await _restaurantReadRepository.FirstOrDefaultAsync(new GetRestaurantWithMenuItemsSpec(restaurantId));
            if (order is null)
            {
                throw new RestaurantException($"Restaurant with id {restaurantId} not found");
            }
        
            var restaurantDto = new RestaurantDto
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Zipcode = order.Zipcode,
                Menu = order.Menu.Select(item => new MenuItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description
                }).ToList()
            };
            return restaurantDto;
        
        }
        catch (Exception ex)
        {
            await _logger.LogToFile(LogLevel.Error, "Something went wrong trying to get a restaurant by id.", ex);
            throw;
        }
    }
    

    public async Task<CreateMenuItemDto> UpdateMenuItemAsync(long restaurantId, long menuItemId, CreateMenuItemDto dto)
    {
        try
        {
            //Get the restaurant we want to update the menu item from
            var restaurant =
                await _restaurantRepository.FirstOrDefaultAsync(new GetRestaurantWithMenuItemsSpec(restaurantId));
            if (restaurant is null)
            {
                throw new RestaurantException($"Restaurant with id {restaurantId} not found");
            }

            //Update the menu item from the restaurant
            var menuItem = restaurant.Menu.FirstOrDefault(x => x.Id == menuItemId);
            if (menuItem is null)
            {
                throw new RestaurantException($"Menu item with id {menuItemId} not found");
            }

            menuItem.Name = dto.Name;
            menuItem.Price = dto.Price;
            menuItem.Description = dto.Description;

            //Update the restaurant
            await _restaurantRepository.UpdateAsync(restaurant);

            //Return the updated restaurant to reflect the new menu
        
            var updatedMenuItem = new CreateMenuItemDto
            {
                Name = menuItem.Name,
                Price = menuItem.Price,
                Description = menuItem.Description
            };
        
            return updatedMenuItem;
            
        }
        catch (Exception ex)
        {
            await _logger.LogToFile(LogLevel.Error, "Something went wrong trying to update a menu item from a restaurant.", ex);
            throw;
        }
    }
}