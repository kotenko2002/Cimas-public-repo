using FluentValidation;

namespace Cimas.Application.Features.Cinemas.Commands.DeleteCinema
{
    public class DeleteCinemaCommandValidator : AbstractValidator<DeleteCinemaCommand>
    {
        public DeleteCinemaCommandValidator()
        {
            RuleFor(x => x.CinemaId)
                .NotEmpty();
        }
    }
}
