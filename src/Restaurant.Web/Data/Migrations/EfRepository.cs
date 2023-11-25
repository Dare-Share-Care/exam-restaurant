using Ardalis.Specification.EntityFrameworkCore;
using Restaurant.Web.Data;
using Restaurant.Web.Interface.Repositories;

namespace Restaurant.Web.Migrations;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    public readonly RestaurantContext RestaurantContext;

    public EfRepository(RestaurantContext restaurantContext) : base(restaurantContext) =>
        this.RestaurantContext = restaurantContext;
}