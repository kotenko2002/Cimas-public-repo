using Cimas.Domain.Entities.Films;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Films.Queries.GetFilmsByCinemaId
{
    public record GetFilmsByCinemaIdQuery(
        Guid UserId,
        Guid CinemaId
    ) : IRequest<ErrorOr<List<Film>>>;
}
