using Restaurant.Web.models.Dto;

namespace Restaurant.Web.Interface.DomainServices;

public interface IRestaurantService
{
     Task<CreateRestaurantDto> CreateRestaurantAsync(string name, string address, int zipcode);
     Task<List<MenuItemDto>> GetRestaurantMenuAsync(long restaurantId);
     Task<RestaurantDto> AddMenuItemAsync(long restaurantId, CreateMenuItemDto dto);
     Task<RestaurantDto> RemoveMenuItemAsync(long restaurantId, long menuItemId);
     Task<List<RestaurantDto>> GetAllRestaurantsAsync();

}