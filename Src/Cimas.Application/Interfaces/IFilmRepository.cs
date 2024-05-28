using Cimas.Domain.Entities.Films;

namespace Cimas.Application.Interfaces
{
    public interface IFilmRepository : IBaseRepository<Film>
    {
        Task<Film> GetFilmIncludedCinemaByIdAsync(Guid filmId);
        Task<List<Film>> GetFilmsByCinemaIdAsync(Guid cinemaId);
    }
}
