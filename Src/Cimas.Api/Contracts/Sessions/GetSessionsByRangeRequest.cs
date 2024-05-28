namespace Cimas.Api.Contracts.Sessions
{
    public record GetSessionsByRangeRequest(
        Guid CinemaId,
        DateTime FromDateTime,
        DateTime ToDateTime
    );
}
