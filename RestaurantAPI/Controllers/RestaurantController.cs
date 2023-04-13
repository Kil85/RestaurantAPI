using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _service = restaurantService;
        }

        [HttpGet]
        [Authorize("RestaurantOwners")]
        public ActionResult<IEnumerable<Restaurant>> GetAll([FromQuery] RestaurantQuery query)
        {
            var result = _service.GetAll(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get([FromRoute] int id)
        {
            var result = _service.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto restaurant)
        {
            var userId = int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);
            var result = _service.CreateRestaurant(restaurant);

            return Created($"/api/restaurant/{result}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult<Restaurant> Delete([FromRoute] int id)
        {
            var result = _service.Delete(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult<Restaurant> Update([FromRoute] int id, [FromBody] UpdateClass update)
        {
            var result = _service.Update(update, id);
            return Ok(result);
        }
    }
}
