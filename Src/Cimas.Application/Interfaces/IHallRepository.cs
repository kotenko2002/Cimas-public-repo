using Cimas.Domain.Entities.Halls;

namespace Cimas.Application.Interfaces
{
    public interface IHallRepository : IBaseRepository<Hall>
    {
        Task<Hall> GetHallIncludedCinemaByIdAsync(Guid hallId);
        Task<Hall> GetHallIncludedAllDependentDataByIdAsync(Guid hallId);
        Task<Hall> GetHallIncludedCinemaAndSeatsByIdAsync(Guid hallId);
        Task<List<Hall>> GetHallsByCinemaIdIncludedSeatsAsync(Guid cinemaId);
    }
}
