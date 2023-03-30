using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterAccountDto>
    {
        public RegisterUserValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.FirstName).NotEmpty();

            RuleFor(x => x.Mail)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(y => y.Password);

            RuleFor(x => x.Mail).Custom((value, context) =>
            {
                var result = dbContext.Users.Any(y => y.Mail == value);
                if (result)
                {
                    context.AddFailure("Mail", "Mail taken");
                }
            });
        }
    }
}
