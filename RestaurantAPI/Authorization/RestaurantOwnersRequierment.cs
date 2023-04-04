using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class RestaurantOwnersRequierment : IAuthorizationRequirement
    {
        public int MinRestaurantsCreated { get; }

        public RestaurantOwnersRequierment(int minRest)
        {
            MinRestaurantsCreated = minRest;
        }
    }
}
