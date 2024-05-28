namespace Cimas.Api.Contracts.Sessions
{
    public record SessionResponse(
        Guid Id,
        DateTime StartDateTime,
        DateTime EndDateTime,
        decimal Price,
        string HallName,
        string FilmName
    );
}
