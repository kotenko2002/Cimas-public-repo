using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class CinemaRepository : BaseRepository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(CimasDbContext context) : base(context) {}

        public override async Task<Cinema> GetByIdAsync(Guid cinemaId)
        {
            return await Sourse
                .Include(cinema => cinema.Company)
                .FirstOrDefaultAsync(cinema => cinema.Id == cinemaId);
        }

        public async Task<Cinema> GetCinemaIncludedProductsByIdAsync(Guid cinemaId)
        {
            return await Sourse
                .Include(cinema => cinema.Products)
                .FirstOrDefaultAsync(cinema => cinema.Id == cinemaId);
        }

        public async Task<List<Cinema>> GetCinemasByCompanyIdAsync(Guid companyId)
        {
            return await Sourse
                .Where(cinema => cinema.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Cinema> GetCinemaIncludedAllDependentDataByIdAsync(Guid cinemaId)
        {
            return await Sourse
                .Include(cinema => cinema.Products)
                .Include(cinema => cinema.Halls)
                    .ThenInclude(hall => hall.Seats)
                        .ThenInclude(seat => seat.Tickets)
                .Include(cinema => cinema.Films)
                    .ThenInclude(film => film.Sessions)
                .Include(cinema => cinema.WorkDays)
                    .ThenInclude(workday => workday.Report)
                .FirstOrDefaultAsync(cinema => cinema.Id == cinemaId);
        }
    }
}
