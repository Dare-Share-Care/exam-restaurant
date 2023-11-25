namespace Restaurant.Web.Exceptions;

public class RestaurantException : Exception
{
    public RestaurantException(string message) : base(message)
    {
    }
}