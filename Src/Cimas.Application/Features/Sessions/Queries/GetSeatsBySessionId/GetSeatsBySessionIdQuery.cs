using Cimas.Domain.Models.Sessions;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Sessions.Queries.GetSeatsBySessionId
{
    public record GetSeatsBySessionIdQuery(
        Guid UserId,
        Guid SessionId
    ) : IRequest<ErrorOr<List<SessionSeat>>>;
}
