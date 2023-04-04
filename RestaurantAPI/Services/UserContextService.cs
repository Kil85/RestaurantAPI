using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal GetUser { get; }
        int? GetUserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _accessor;

        public UserContextService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public ClaimsPrincipal GetUser => _accessor.HttpContext.User;
        public int? GetUserId =>
            GetUser == null
                ? null
                : int.Parse(GetUser.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
