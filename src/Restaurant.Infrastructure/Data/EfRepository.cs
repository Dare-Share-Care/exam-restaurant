using Ardalis.Specification.EntityFrameworkCore;
using Restaurant.Infrastructure.Interfaces;

namespace Restaurant.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    public readonly RestaurantContext RestaurantContext;

    public EfRepository(RestaurantContext restaurantContext) : base(restaurantContext) =>
        this.RestaurantContext = restaurantContext;
}