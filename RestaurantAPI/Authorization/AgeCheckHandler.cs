using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class AgeCheckHandler : AuthorizationHandler<AgeCheckRequirement>
    {
        private readonly ILogger _logger;

        public AgeCheckHandler(ILogger<AgeCheckHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AgeCheckRequirement requirement
        )
        {
            var dateOfBirth = DateTime.Parse(
                context.User.Claims.FirstOrDefault(c => c.Type == "DateOfBirth").Value
            );

            var userName = context.User.Claims.FirstOrDefault(c => c.Type == "Name");

            _logger.LogInformation($"Checking if user {userName} is {requirement.RequiredAge}");

            if (dateOfBirth.AddYears(requirement.RequiredAge) < DateTime.UtcNow)
            {
                _logger.LogInformation("Authorization compled");

                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization faild");
            }

            return Task.CompletedTask;
        }
    }
}
