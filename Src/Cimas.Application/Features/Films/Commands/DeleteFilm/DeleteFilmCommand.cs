using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Films.Commands.DeleteFilm
{
    public record DeleteFilmCommand(
        Guid UserId,
        Guid FilmId
    ) : IRequest<ErrorOr<Success>>;
}
