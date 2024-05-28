using FluentValidation;

namespace Cimas.Application.Features.Halls.Commands.DeleteHall
{
    public class DeleteHallCommandValidator : AbstractValidator<DeleteHallCommand>
    {
        public DeleteHallCommandValidator()
        {
            RuleFor(x => x.HallId)
                .NotEmpty();
        }
    }
}
