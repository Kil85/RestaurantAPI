using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantID}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService service)
        {
            _dishService = service;
        }

        [HttpPost]
        public ActionResult AddDish([FromRoute] int restaurantID, [FromBody] CreateDishDto dto)
        {

            var result = _dishService.Add(restaurantID, dto);

            return Created($"api/restaurant/{restaurantID}/dish/{result}", null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> GetById([FromRoute] int restaurantID, [FromRoute] int dishId)
        {
            var result = _dishService.GetById(restaurantID, dishId);

            return Ok(result);
        }
        [HttpGet]
        public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            var result = _dishService.GetAll(restaurantId);
            return Ok(result);
        }

        [HttpDelete("{dishId}")]
        public ActionResult<DishDto> Delete([FromRoute] int restaurantID, [FromRoute] int dishId)
        {
            var result = _dishService.Delete(restaurantID, dishId);
            return Ok(result);
        }

    }
}
