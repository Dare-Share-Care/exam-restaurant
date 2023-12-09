using Restaurant.Core.Models.Dto;
using Restaurant.Core.Models.ViewModels;

namespace Restaurant.Core.Interfaces;

public interface IRestaurantService
{
     Task<RestaurantViewModel> CreateRestaurantAsync(CreateRestaurantDto dto);
     Task<List<MenuItemDto>> GetRestaurantMenuAsync(long restaurantId);
     Task<RestaurantDto> AddMenuItemAsync(long restaurantId, CreateMenuItemDto dto);
     Task<RestaurantDto> RemoveMenuItemAsync(long restaurantId, long menuItemId);
     Task<List<RestaurantViewModel>> GetAllRestaurantsAsync();
     Task<CreateMenuItemDto> UpdateMenuItemAsync(long restaurantId, long menuItemId, CreateMenuItemDto dto);

     Task<RestaurantDto> GetRestaurantById(long restaurantId);
}