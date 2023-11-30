using HotChocolate.Types;
using Restaurant.Infrastructure.Entities;

namespace Restaurant.Core.Types
{
    public class RestaurantType : ObjectType<Restauranten>
    {
        protected override void Configure(IObjectTypeDescriptor<Restauranten> descriptor)
        {
            descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
            descriptor.Field(r => r.Name).Type<NonNullType<StringType>>();
            descriptor.Field(r => r.Address).Type<StringType>();
            descriptor.Field(r => r.Zipcode).Type<IntType>();
            // Add other fields as needed
        }
    }
}