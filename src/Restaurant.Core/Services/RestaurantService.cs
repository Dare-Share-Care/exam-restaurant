﻿using Microsoft.Extensions.Logging;
using Restaurant.Core.Interfaces;
using Restaurant.Core.Models.Dto;
using Restaurant.Core.Models.ViewModels;
using Restaurant.Core.Exceptions;
using Restaurant.Infrastructure.Entities;
using Restaurant.Infrastructure.Interfaces;
using Restaurant.Infrastructure.Specifications;
using Path = System.IO.Path;

namespace Restaurant.Core.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IReadRepository<Restauranten> _restaurantReadRepository;
    private readonly IRepository<Restauranten> _restaurantRepository;
    private readonly ILogger _logger;

    public RestaurantService(IReadRepository<Restauranten> restaurantReadRepository, IRepository<Restauranten> restaurantRepository, ILogger<Restauranten> logger)
    {
        _restaurantReadRepository = restaurantReadRepository;
        _restaurantRepository = restaurantRepository;
        _logger = logger;
    }

    public async Task<RestaurantViewModel> CreateRestaurantAsync(CreateRestaurantDto dto)
    {
        var createdRestaurant = await _restaurantRepository.AddAsync(new Restauranten
        {
            Name = dto.Name,
            Address = dto.Address,
            Zipcode = dto.Zipcode
        });

        var restaurantDto = new RestaurantViewModel()
        {
            Id = createdRestaurant.Id,
            Name = createdRestaurant.Name,
            Address = createdRestaurant.Address,
            Zipcode = createdRestaurant.Zipcode
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
        try
        {
            
            throw new ("This msg is on purpose");
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
        catch (Exception e)
        {
            _logger.LogError("Something went wrong when getting all restaurants: {0}", e.Message);

            // Log to a text file
            LogToFile($"Error: {e.Message}\nStackTrace: {e.StackTrace}");

            throw;
        }
    }

    private void LogToFile(string logMessage)
    {
        try
        {
            // Specify your log file path
            string logFilePath = "logs/logfile.txt";
            
            // Ensure the directory exists before logging
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            
            // Append the log message to the file
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {logMessage}\n");
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during file logging
            _logger.LogError("Error logging to file: {0}", ex.Message);
        }
    }

    public async Task<CreateMenuItemDto> UpdateMenuItemAsync(long restaurantId, long menuItemId, CreateMenuItemDto dto)
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
}