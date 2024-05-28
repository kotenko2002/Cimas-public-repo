using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Users.Commands.RegisterNonOwner
{
    public record RegisterNonOwnerCommand(
        Guid OwnerUserId,
        string FirstName,
        string LastName,
        string Username,
        string Password,
        string Role
    ) : IRequest<ErrorOr<Success>>;
}
