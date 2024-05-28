using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Tickets;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(CimasDbContext context) : base(context) {}

        public async Task<List<Ticket>> GetTicketsBySessionIdAsync(Guid sessionId)
        {
            return await Sourse
                .Where(ticket => ticket.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketsByIdsAsync(List<Guid> ids)
        {
            return await Sourse
               .Where(seat => ids.Contains(seat.Id))
               .ToListAsync();
        }

        public async Task<bool> TicketsAlreadyExistsAsync(Guid sessionId, List<Guid> seatIds)
        {
            return await Sourse
                .Where(ticket => ticket.SessionId == sessionId)
                .Select(ticket => ticket.SeatId)
                .AnyAsync(seatId => seatIds.Contains(seatId));
        }

        public async Task<List<Ticket>> GetTicketsByDateTimeRangeAndCinemaIdAsync(DateTime from, DateTime to, Guid cinemaId)
        {
            return await Sourse
                .Include(ticket => ticket.Session)
                    .ThenInclude(ticket => ticket.Film)
                .Where(ticket => ticket.Session.Film.CinemaId == cinemaId
                    && ticket.CreationTime > from && ticket.CreationTime < to)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketsWithFullIncludesForPdf(List<Guid> ids)
        {
            return await Sourse
                .Include(ticket => ticket.Seat)
                .Include(ticket => ticket.Session)
                    .ThenInclude(session => session.Film)
                .Include(ticket => ticket.Session)
                    .ThenInclude(session => session.Hall)
                        .ThenInclude(hall => hall.Cinema)
               .Where(seat => ids.Contains(seat.Id))
               .ToListAsync();
        }
    }
}
