using Restaurant.Infrastructure.Data;

namespace Restaurant.Core.Types
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("restaurants")
                .Type<ListType<RestaurantType>>() // Assuming you have a RestaurantType
                .Resolve(context =>
                {
                    var dbContext = context.Service<RestaurantContext>();
                    return dbContext.Restaurant.ToList();
                });
        }
    }
}