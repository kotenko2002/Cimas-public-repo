using Cimas.Domain.Entities.Halls;

namespace Cimas.Application.Interfaces
{
    public interface ISeatRepository : IBaseRepository<HallSeat>
    {
        Task<List<HallSeat>> GetSeatsByHallIdAsync(Guid hallId);
        Task<List<HallSeat>> GetSeatsByIdsAsync(IEnumerable<Guid> ids);
    }
}
