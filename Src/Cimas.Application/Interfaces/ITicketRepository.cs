using Cimas.Domain.Entities.Tickets;

namespace Cimas.Application.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketsBySessionIdAsync(Guid sessionId);
        Task<List<Ticket>> GetTicketsByIdsAsync(List<Guid> ids);
        Task<bool> TicketsAlreadyExistsAsync(Guid sessionId, List<Guid> seatIds);
        Task<List<Ticket>> GetTicketsByDateTimeRangeAndCinemaIdAsync(DateTime from, DateTime to, Guid cinemaId);
        Task<List<Ticket>> GetTicketsWithFullIncludesForPdf(List<Guid> ids); 
    }
}
