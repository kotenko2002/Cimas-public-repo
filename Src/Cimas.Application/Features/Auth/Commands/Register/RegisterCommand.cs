using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(
        Guid CompanyId,
        string FirstName,
        string LastName,
        string Username,
        string Password,
        string Role
    ) : IRequest<ErrorOr<Success>>;
}
