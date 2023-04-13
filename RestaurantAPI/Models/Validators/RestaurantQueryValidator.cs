using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortBy = new[]
        {
            nameof(Restaurant.Name),
            nameof(Restaurant.Description),
            nameof(Restaurant.CreatedBy)
        };

        public RestaurantQueryValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty();
            RuleFor(x => x.PageSize)
                .Custom(
                    (value, context) =>
                    {
                        if (!allowedPageSizes.Contains(value))
                        {
                            context.AddFailure("PageSize", "Page Size Not Allowed");
                        }
                    }
                );
            RuleFor(x => x.SortBy)
                .Must(s => string.IsNullOrEmpty(s) || allowedSortBy.Contains(s))
                .WithMessage($"Sort by must be null or [{string.Join(", ", allowedSortBy)}]");
        }
    }
}
