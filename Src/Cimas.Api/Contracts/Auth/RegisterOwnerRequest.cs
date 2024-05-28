namespace Cimas.Api.Contracts.Auth
{
    public record RegisterOwnerRequest(
        string CompanyName,
        string FirstName,
        string LastName,
        string Username,
        string Password
    );
}
