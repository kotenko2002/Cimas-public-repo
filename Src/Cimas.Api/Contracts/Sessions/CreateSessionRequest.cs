namespace Cimas.Api.Contracts.Sessions
{
    public record CreateSessionRequest(
        Guid HallId,
        Guid FilmId,
        DateTime StartTime,
        decimal Price
    );
}
