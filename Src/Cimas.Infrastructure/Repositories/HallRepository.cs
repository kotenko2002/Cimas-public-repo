using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Halls;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class HallRepository : BaseRepository<Hall>, IHallRepository
    {
        public HallRepository(CimasDbContext context) : base(context) {}

        public async Task<Hall> GetHallIncludedCinemaByIdAsync(Guid hallId)
        {
            return await Sourse
                .Include(hall => hall.Cinema)
                .FirstOrDefaultAsync(hall => hall.Id == hallId);
        }

        public async Task<Hall> GetHallIncludedAllDependentDataByIdAsync(Guid hallId)
        {
            return await Sourse
                .Include(hall => hall.Seats)
                .Include(hall => hall.Sessions)
                    .ThenInclude(session => session.Tickets)
                .FirstOrDefaultAsync(hall => hall.Id == hallId);
        }

        public async Task<Hall> GetHallIncludedCinemaAndSeatsByIdAsync(Guid hallId)
        {
            return await Sourse
                .Include(hall => hall.Cinema)
                .Include(hall => hall.Seats)
                .FirstOrDefaultAsync(hall => hall.Id == hallId);
        }

        public async Task<List<Hall>> GetHallsByCinemaIdIncludedSeatsAsync(Guid cinemaId)
        {
            return await Sourse
                .Include(hall => hall.Seats)
                .Where(hall => hall.CinemaId == cinemaId)
                .ToListAsync();
        }
    }
}
