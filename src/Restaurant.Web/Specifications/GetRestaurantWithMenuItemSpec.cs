using Ardalis.Specification;
using Restaurant.Web.Entities;

namespace Restaurant.Web.Specifications;

public sealed class GetRestaurantWithMenuItemsSpec : Specification<Restauranten>
{
    public GetRestaurantWithMenuItemsSpec(long restaurantId)
    {
        Query.Where(res => res.Id == restaurantId)
            .Include(res => res.Menu);
    }
}