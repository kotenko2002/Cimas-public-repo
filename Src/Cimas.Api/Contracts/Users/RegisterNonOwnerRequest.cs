namespace Cimas.Api.Contracts.Users
{
    public record RegisterNonOwnerRequest(
        string FirstName,
        string LastName,
        string Username,
        string Password,
        string Role
    );
}
