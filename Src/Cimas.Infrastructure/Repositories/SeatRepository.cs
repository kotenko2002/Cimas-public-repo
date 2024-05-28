using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Halls;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class SeatRepository : BaseRepository<HallSeat>, ISeatRepository
    {
        public SeatRepository(CimasDbContext context) : base(context) {}

        public async Task<List<HallSeat>> GetSeatsByHallIdAsync(Guid hallId)
        {
            return await Sourse
               .Where(seat => seat.HallId == hallId)
               .ToListAsync();
        }

        public async Task<List<HallSeat>> GetSeatsByIdsAsync(IEnumerable<Guid> ids)
        {
            return await Sourse
               .Where(seat => ids.Contains(seat.Id))
               .ToListAsync();
        }
    }
}
