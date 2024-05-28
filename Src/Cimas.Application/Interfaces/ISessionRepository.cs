using Cimas.Domain.Entities.Sessions;

namespace Cimas.Application.Interfaces
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<Session> GetSessionIncludedTicketsByIdAsync(Guid sessionId);
        Task<List<Session>> GetSessionsByRangeAsync(Guid cinemaId, DateTime fromDateTime, DateTime toDateTime);
        Task<Session> GetSessionsIncludedHallThenIncludedCinemaByIdAsync(Guid sessionId);
        Task<bool> IsSessionCollisionDetectedAsync(Guid hallId, DateTime newSessionStartDateTime, DateTime newSessionEndDateTime);
    }
}
