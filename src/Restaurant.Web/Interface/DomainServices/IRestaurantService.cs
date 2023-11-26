using Restaurant.Web.models.Dto;
using Restaurant.Web.Models.ViewModels;

namespace Restaurant.Web.Interface.DomainServices;

public interface IRestaurantService
{
     Task<RestaurantViewModel> CreateRestaurantAsync(CreateRestaurantDto dto);
     Task<List<MenuItemDto>> GetRestaurantMenuAsync(long restaurantId);
     Task<RestaurantDto> AddMenuItemAsync(long restaurantId, CreateMenuItemDto dto);
     Task<RestaurantDto> RemoveMenuItemAsync(long restaurantId, long menuItemId);
     Task<List<RestaurantDto>> GetAllRestaurantsAsync();
     Task<CreateMenuItemDto> UpdateMenuItemAsync(long restaurantId, long menuItemId, CreateMenuItemDto dto);

}