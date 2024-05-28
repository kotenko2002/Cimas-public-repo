using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Commands.CreateHall
{
    public record CreateHallCommand(
        Guid UserId,
        Guid CinemaId,
        string Name,
        int NumberOfRows,
        int NumberOfColumns
    ) : IRequest<ErrorOr<Success>>;
}
