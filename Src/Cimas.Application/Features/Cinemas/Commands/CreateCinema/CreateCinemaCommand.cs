using Cimas.Domain.Entities.Cinemas;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Cinemas.Commands.CreateCinema
{
    public record CreateCinemaCommand(
        Guid UserId,
        string Name,
        string Address
    ) : IRequest<ErrorOr<Cinema>>;
}
