using FluentValidation;

namespace Cimas.Application.Features.Cinemas.Commands.CreateCinema
{
    public class CreateCinemaCommandValidator : AbstractValidator<CreateCinemaCommand>
    {
        public CreateCinemaCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(100);
        }
    }
}
