using Grpc.Core;
using Restaurant.Web.Interface.DomainServices;

namespace Restaurant.Grpc.Services
{
    public class CatalogueService : Catalogue.CatalogueBase
    {
        private readonly IServiceProvider _serviceProvider;

        public CatalogueService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<RestaurantResponse> SendCatalogue(RestaurantRequest request, ServerCallContext context)
        {
                var restaurantResponse = new RestaurantResponse
                {
                    Restaurant = new Restauranten
                    {
                        Id = 1,
                        Menu = { }
                    }
                };
               
                    var menuItemProto = new MenuItem
                    {
                        Id = 1,
                        Name = "banan",
                        Price = 25
                    };
                    
                    restaurantResponse.Restaurant.Menu.Add(menuItemProto);
                
                return restaurantResponse;
            }
        }
    }
