using Grpc.Core;
using Restaurant.Core.Interfaces;

namespace Restaurant.Grpc.Services
{
    public class CatalogueService : Catalogue.CatalogueBase
    {
        private readonly IRestaurantService _restaurantService;
        
        public CatalogueService(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        public override async Task<RestaurantResponse> SendCatalogue(RestaurantRequest request,
            ServerCallContext context)
        {
            // Fetch the restaurants
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            // Find the restaurant matching the request
            var restaurant = restaurants.FirstOrDefault(r => r.Id == request.RestaurantId);
            
            // Get menu 
            var menu = await _restaurantService.GetRestaurantMenuAsync(request.RestaurantId);
            restaurant.Menu = menu;
            
            // Check if the restaurant was found
            if (restaurant != null)
            {
                // Convert menu items to protobuf format
                var menuItems = restaurant.Menu.Select(item => new MenuItem
                    { Id = item.Id, Name = item.Name, Price = (float)item.Price }).ToList();

                // Create the restaurant protobuf message
                var restaurantMessage = new Restauranten { Id = restaurant.Id, Menu = { menuItems } };

                // Return the response
                return new RestaurantResponse { Restaurant = restaurantMessage };
            }
            else
            {
                // Restaurant not found, return an empty response or throw an error
                return new RestaurantResponse();
            }
        }
    }
}