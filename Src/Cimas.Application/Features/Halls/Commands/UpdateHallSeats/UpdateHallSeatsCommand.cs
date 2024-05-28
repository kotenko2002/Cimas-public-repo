using Cimas.Domain.Entities.Halls;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Commands.UpdateHallSeats
{
    public record UpdateHallSeatsCommand(
        Guid UserId,
        Guid HallId,
        List<UpdateSeat> Seats
    ) : IRequest<ErrorOr<Success>>;

    public record UpdateSeat(
        Guid Id,
        HallSeatStatus Status
    );
}
