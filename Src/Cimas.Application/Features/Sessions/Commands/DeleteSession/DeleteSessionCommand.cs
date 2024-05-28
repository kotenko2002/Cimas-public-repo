using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Sessions.Commands.DeleteSession
{
    public record DeleteSessionCommand(
        Guid UserId,
        Guid SessionId
    ) : IRequest<ErrorOr<Success>>;
}
