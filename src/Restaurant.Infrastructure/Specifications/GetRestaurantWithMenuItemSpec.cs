using Ardalis.Specification;
using Restaurant.Infrastructure.Entities;

namespace Restaurant.Infrastructure.Specifications;

public sealed class GetRestaurantWithMenuItemsSpec : Specification<Restauranten>
{
    public GetRestaurantWithMenuItemsSpec(long restaurantId)
    {
        Query.Where(res => res.Id == restaurantId)
            .Include(res => res.Menu);
    }
}