using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class AgeCheckRequirement : IAuthorizationRequirement
    {
        public int RequiredAge { get;}

        public AgeCheckRequirement(int Age)
        {
            RequiredAge = Age;
        }
    }
}
