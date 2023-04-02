using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationHandler
        : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ResourceOperationRequirement requirement,
            Restaurant resource
        )
        {

            return Task.CompletedTask;
        }
    }

}
