using FluentValidation;

namespace Cimas.Application.Features.Users.Commands.FireUser
{
    public class FireUserValidator : AbstractValidator<FireUserCommand>
    {
        public FireUserValidator()
        {

            RuleFor(x => x.UserToFireId)
                .NotEmpty();
        }
    }
}
