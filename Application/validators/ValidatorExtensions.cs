using FluentValidation;

namespace Application.validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder.NotEmpty()
                .MinimumLength(6).WithMessage("6 characters at least")
                .Matches("[A-Z]").WithMessage("at least one uppercase letter")
                .Matches("[a-z]").WithMessage("at least one lower case")
                .Matches("[0-9]").WithMessage("at least on number");

            return options;
        }
    }
}