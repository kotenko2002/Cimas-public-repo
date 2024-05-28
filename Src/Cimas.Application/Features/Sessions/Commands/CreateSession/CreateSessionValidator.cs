using FluentValidation;

namespace Cimas.Application.Features.Sessions.Commands.CreateSession
{
    public class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionValidator()
        {
            RuleFor(x => x.HallId)
                .NotEmpty();

            RuleFor(x => x.FilmId)
                .NotEmpty();
        }
    }
}
