using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class RestaurantOwnersHandler : AuthorizationHandler<RestaurantOwnersRequierment>
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantOwnersHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RestaurantOwnersRequierment requirement
        )
        {
            var userId = int.Parse(
                context.User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value
            );

            var count = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

            if (count >= requirement.MinRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
