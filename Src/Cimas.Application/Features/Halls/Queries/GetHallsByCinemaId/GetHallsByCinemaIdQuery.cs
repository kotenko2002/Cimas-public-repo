using Cimas.Domain.Entities.Halls;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Queries.GetHallsByCinemaId
{
    public record GetHallsByCinemaIdQuery(
        Guid UserId,
        Guid CinemaId
    ) : IRequest<ErrorOr<List<Hall>>>;
}
