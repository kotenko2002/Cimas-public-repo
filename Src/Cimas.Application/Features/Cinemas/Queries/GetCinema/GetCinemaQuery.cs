using Cimas.Domain.Entities.Cinemas;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Queries.GetCinema
{
    public record GetCinemaQuery(
        Guid UserId,
        Guid CinemaId
    ) : IRequest<ErrorOr<Cinema>>;
}
