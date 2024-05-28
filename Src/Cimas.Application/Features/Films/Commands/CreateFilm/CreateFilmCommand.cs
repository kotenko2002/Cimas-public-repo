using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Films.Commands.CreateFilm
{
    public record CreateFilmCommand(
        Guid UserId,
        Guid CinemaId,
        string Name,
        TimeSpan Duration
    ) : IRequest<ErrorOr<Success>>;
}
