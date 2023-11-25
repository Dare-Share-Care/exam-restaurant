﻿using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.models.Dto;
using Restaurant.Web.Interface.DomainServices;

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
            //Map to viewmodel
            var model = restaurants.Select(restaurant => new RestaurantDto()
            {
                Id = restaurant.Id,
                Name = restaurant.Name
            }).ToList();
            return Ok(model);
        }
        
        //Get menu for a restaurant
        [HttpGet("{restaurantId}/menu")]
        public async Task<IActionResult> GetRestaurantMenu(long restaurantId)
        {
            var menu = await _restaurantService.GetRestaurantMenuAsync(restaurantId);
            return Ok(menu);
        }

        [HttpPost("create-restaurant/{restaurantId}")]
        public async Task<IActionResult> CreateRestaurant([FromBody] string name)
        {
            var restaurant = await _restaurantService.CreateRestaurantAsync(name);
            return Ok(restaurant);
        }

        [HttpPost("add-menuitem/{restaurantId}")]
        public async Task<IActionResult> AddMenuItem(long restaurantId, [FromBody] CreateMenuItemDto dto)
        {
            var restaurant = await _restaurantService.AddMenuItemAsync(restaurantId, dto);
            return Ok(restaurant);
        }

        [HttpDelete("remove-menuitem/{restaurantId}")]
        public async Task<IActionResult> RemoveMenuItem(long restaurantId, [FromBody] long menuItemId)
        {
            var restaurant = await _restaurantService.RemoveMenuItemAsync(restaurantId, menuItemId);
            return Ok(restaurant);
        }
        
}