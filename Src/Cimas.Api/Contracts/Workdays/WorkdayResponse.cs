namespace Cimas.Api.Contracts.Workdays
{
    public record WorkdayResponse(
        Guid Id,
        DateTime StartDateTime,
        DateTime? EndDateTime,
        Guid CinemaId,
        Guid UserId
    );
}
