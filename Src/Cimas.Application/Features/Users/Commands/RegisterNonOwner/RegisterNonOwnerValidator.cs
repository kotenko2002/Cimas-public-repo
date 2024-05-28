using Cimas.Application.Common.Extensions;
using Cimas.Domain.Entities.Users;
using FluentValidation;

namespace Cimas.Application.Features.Users.Commands.RegisterNonOwner
{
    public class RegisterNonOwnerValidator : AbstractValidator<RegisterNonOwnerCommand>
    {
        public RegisterNonOwnerValidator()
        {
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
                .Must(role => role.IsNonOwner())
                .WithMessage(GenerateNonValidRoleErrorMessage);
        }

        private string GenerateNonValidRoleErrorMessage(RegisterNonOwnerCommand command)
            => command.Role.GenerateNonValidRoleErrorMessage(Roles.GetNonOwnerRoles());
    }
}
