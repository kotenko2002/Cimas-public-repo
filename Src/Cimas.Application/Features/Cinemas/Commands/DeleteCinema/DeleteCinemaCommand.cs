using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Commands.DeleteCinema
{
    public record DeleteCinemaCommand(
        Guid UserId,
        Guid CinemaId
    ) : IRequest<ErrorOr<Success>>;
}
