﻿using Microsoft.Extensions.Logging;
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
        catch (Exception ex)
        {
            // Log an error with an exception
            _logger.LogToFile(LogLevel.Error, "Something went wrong.", ex);

            // Log a debug message
            _logger.LogToFile(LogLevel.Debug, "Debugging information.");

            // Log an information message
            _logger.LogToFile(LogLevel.Information, "Application started.");

            // Log a warning
            _logger.LogToFile(LogLevel.Warning, "Potential issue detected.");
            throw;
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