using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _controller;

        public AccountController(IAccountService controller)
        {
            _controller = controller;
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterAccountDto dto)
        {
            _controller.Register(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            var result = _controller.Login(dto);
            return Ok(result);
        }
    }
}
