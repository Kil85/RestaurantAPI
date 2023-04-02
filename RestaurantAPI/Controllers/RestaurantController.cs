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
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _service = restaurantService;
        }

        [HttpGet]
        [Authorize("AgeCheck")]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var result = _service.GetAll();

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
            var result = _service.CreateRestaurant(restaurant, userId);

            return Created($"/api/restaurant/{result}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult<Restaurant> Delete([FromRoute] int id)
        {
            var userId = int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

            var result = _service.Delete(id, userId);
            return Ok(result);
        }

        [HttpPut]
        public ActionResult<Restaurant> Update([FromBody] UpdateClass update)
        {
            var userId = int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

            var result = _service.Update(update, userId);
            return Ok(result);
        }
    }
}
