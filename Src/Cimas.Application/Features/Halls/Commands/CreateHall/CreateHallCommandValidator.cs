using FluentValidation;

namespace Cimas.Application.Features.Halls.Commands.CreateHall
{
    public class CreateHallCommandValidator : AbstractValidator<CreateHallCommand>
    {
        public CreateHallCommandValidator()
        {
            RuleFor(x => x.CinemaId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.NumberOfRows)
                .NotEmpty()
                .InclusiveBetween(1, 30);

            RuleFor(x => x.NumberOfColumns)
                .NotEmpty()
                .InclusiveBetween(1, 30);
        }
    }
}
