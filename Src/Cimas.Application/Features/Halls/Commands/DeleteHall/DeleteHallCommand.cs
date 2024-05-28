using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Halls.Commands.DeleteHall
{
    public record DeleteHallCommand(
      Guid UserId,
      Guid HallId
    ) : IRequest<ErrorOr<Success>>;
}
