using FluentValidation;

namespace Cimas.Application.Features.Auth.Commands.RegisterOwner
{
    public class RegisterOwnerValidator : AbstractValidator<RegisterOwnerCommand>
    {
        public RegisterOwnerValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(15);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20);
        }
    }
}
