using FluentValidation;

namespace Cimas.Application.Features.Films.Commands.CreateFilm
{
    public class CreateFilmValidator : AbstractValidator<CreateFilmCommand>
    {
        public CreateFilmValidator()
        {
            RuleFor(x => x.CinemaId)
               .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.Duration)
                .NotEmpty()
                .Must(d => d.TotalMinutes >= 0.1 && d.TotalMinutes <= 300)
                .WithMessage("Duration must be between 0.1 and 300 minutes");
        }
    }
}
