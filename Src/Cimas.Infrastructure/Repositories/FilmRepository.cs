using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Films;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class FilmRepository : BaseRepository<Film>, IFilmRepository
    {
        public FilmRepository(CimasDbContext context) : base(context) {}

        public override async Task<Film> GetByIdAsync(Guid filmId)
        {
            return await Sourse
                .FirstOrDefaultAsync(film => film.Id == filmId && !film.IsDeleted);
        }

        public override async Task<IEnumerable<Film>> GetAllAsync()
        {
            return await Sourse
                .Where(film => !film.IsDeleted)
                .ToListAsync();
        }
      
        public async Task<Film> GetFilmIncludedCinemaByIdAsync(Guid filmId)
        {
            return await Sourse
                .Include(film => film.Cinema)
                .FirstOrDefaultAsync(film => film.Id == filmId && !film.IsDeleted);
        }

        public async Task<List<Film>> GetFilmsByCinemaIdAsync(Guid cinemaId)
        {
            return await Sourse
                .Where(hall => hall.CinemaId == cinemaId && !hall.IsDeleted)
                .ToListAsync();
        }
    }
}
