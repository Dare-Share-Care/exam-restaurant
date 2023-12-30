using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Core.Exceptions;
using Restaurant.Core.Interfaces;
using Restaurant.Core.Models.Dto;

namespace Restaurant.Web.Controller;

    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("get-all-restaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }
        
        //Get menu for a restaurant
        [HttpGet("{restaurantId}/menu")]
        public async Task<IActionResult> GetRestaurantMenu(long restaurantId)
        {
            var menu = await _restaurantService.GetRestaurantMenuAsync(restaurantId);
            return Ok(menu);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost("create-restaurant")]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var restaurant = await _restaurantService.CreateRestaurantAsync(dto);
            return Ok(restaurant);
        }

        [Authorize(Roles = "RestaurantOwner")]
        [HttpPost("create-menuitem/{restaurantId}")]
        public async Task<IActionResult> AddMenuItem(long restaurantId, [FromBody] CreateMenuItemDto dto)
        {
            var restaurant = await _restaurantService.AddMenuItemAsync(restaurantId, dto);
            return Ok(restaurant);
        }
        
        [Authorize(Roles = "RestaurantOwner")]
        [HttpDelete("remove-menuitem/{restaurantId}")]
        public async Task<IActionResult> RemoveMenuItem(long restaurantId, long menuItemId)
        {
            var restaurant = await _restaurantService.RemoveMenuItemAsync(restaurantId, menuItemId);
            return Ok(restaurant);
        }
        
        [Authorize(Roles = "RestaurantOwner")]
        [HttpPut("update-menuitem/{restaurantId}/{menuItemId}")]
        public async Task<IActionResult> UpdateMenuItem(long restaurantId, long menuItemId, [FromBody] CreateMenuItemDto dto)
        {
            try
            {
                var updatedMenuItem = await _restaurantService.UpdateMenuItemAsync(restaurantId, menuItemId, dto);
                return Ok(updatedMenuItem);
            }
            catch (RestaurantException ex)
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }
        }
}