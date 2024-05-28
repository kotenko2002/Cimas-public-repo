using FluentValidation;

namespace Cimas.Application.Features.Films.Commands.DeleteFilm
{
    public class DeleteFilmValidator : AbstractValidator<DeleteFilmCommand>
    {
        public DeleteFilmValidator()
        {
            RuleFor(x => x.FilmId)
               .NotEmpty();
        }
    }
}
