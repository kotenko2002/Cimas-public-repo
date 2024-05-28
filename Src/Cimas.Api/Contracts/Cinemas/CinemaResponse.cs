namespace Cimas.Api.Contracts.Cinemas
{
    public record CinemaResponse(
        Guid Id,
        string Name,
        string Address
    );
}
