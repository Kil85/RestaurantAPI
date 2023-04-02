using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum ResourceOperations
    {
        Read,
        Create,
        Update,
        Delete
    }

    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperations operation { get; }

        public ResourceOperationRequirement(ResourceOperations resourceOperation)
        {
            operation = resourceOperation;
        }
    }
}
