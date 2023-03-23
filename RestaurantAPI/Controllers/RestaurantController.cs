using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _service = restaurantService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var result = _service.GetAll();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get([FromRoute] int id)
        {
            var result = _service.GetById(id);

            if (result == null) return NotFound();


            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreateRestauration([FromBody] CreateRestaurantDto restaurant)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _service.CreateRestaurant(restaurant);

            return Created($"/api/restaurant/{result}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult<Restaurant> Delete([FromRoute] int id)
        {

            var result = _service.Delete(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut]
        public ActionResult<Restaurant> Update([FromBody] UpdateClass update)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var result = _service.Update(update);
            if (result == null) return NotFound();
            return Ok(result);
        }

    }
}
