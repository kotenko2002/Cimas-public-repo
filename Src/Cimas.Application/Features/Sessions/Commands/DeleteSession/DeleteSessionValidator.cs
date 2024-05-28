using FluentValidation;

namespace Cimas.Application.Features.Sessions.Commands.DeleteSession
{
    public class DeleteSessionValidator : AbstractValidator<DeleteSessionCommand>
    {
        public DeleteSessionValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty();
        }
    }
}
