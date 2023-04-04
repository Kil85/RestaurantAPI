using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

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
            if (
                requirement.operation == ResourceOperations.Create
                || requirement.operation == ResourceOperations.Read
            )
            {
                context.Succeed(requirement);
            }

            var userId = int.Parse(
                context.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value
            );
            if (userId == resource.CreatedById)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
