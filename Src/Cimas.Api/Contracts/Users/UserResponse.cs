namespace Cimas.Api.Contracts.Users
{
    public record UserResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string[] Roles
    );
}
