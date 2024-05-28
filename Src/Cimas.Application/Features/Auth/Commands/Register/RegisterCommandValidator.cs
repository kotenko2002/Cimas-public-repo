using Cimas.Application.Common.Extensions;
using Cimas.Domain.Entities.Users;
using FluentValidation;

namespace Cimas.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty();

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

            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(role => role.IsRoleValid())
                .WithMessage(GenerateNonValidRoleErrorMessage);
        }

        private string GenerateNonValidRoleErrorMessage(RegisterCommand command)
            => command.Role.GenerateNonValidRoleErrorMessage(Roles.GetRoles());
    }
}
