using Cimas.Application.Common.Extensions;
using FluentValidation;

namespace Cimas.Application.Features.Halls.Commands.UpdateHallSeats
{
    public class UpdateHallSeatsCommandValidator : AbstractValidator<UpdateHallSeatsCommand>
    {
        public UpdateHallSeatsCommandValidator()
        {
            RuleFor(x => x.HallId)
               .NotEmpty();

            RuleFor(x => x.Seats)
                .NotEmpty()
                .MustHaveUniqueIds(seat => seat.Id)
                .MustBeValidEnum(seat => seat.Status);
        }
    }
}
