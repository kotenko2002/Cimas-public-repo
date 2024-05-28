using Cimas.Domain.Models.Auth;

namespace Cimas.Api.Contracts
{
    public record AuthResponse(
        string AccessToken,
        AuthUserModel User
    );

    public record AuthUserModel(
        string FullName,
        string[] Roles
    );
}
