using Cimas.Domain.Entities.Sessions;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Sessions.Queries.GetSessionsByRange
{
    public record GetSessionsByRangeQuery(
        Guid UserId,
        Guid CinemaId,
        DateTime FromDateTime,
        DateTime ToDateTime
    ) : IRequest<ErrorOr<List<Session>>>;
}
