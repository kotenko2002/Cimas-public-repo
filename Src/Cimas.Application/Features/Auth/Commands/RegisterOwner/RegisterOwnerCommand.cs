using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Auth.Commands.RegisterOwner
{
    public record RegisterOwnerCommand(
        string CompanyName,
        string FirstName,
        string LastName,
        string Username,
        string Password
    ) : IRequest<ErrorOr<Success>>;
}
