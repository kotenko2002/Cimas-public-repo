using Cimas.Domain.Entities.Halls;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Queries.GetSeatsByHallId
{
    public record GetSeatsByHallIdQuery(
        Guid UserId,
        Guid HallId
    ) : IRequest<ErrorOr<List<HallSeat>>>;
}
