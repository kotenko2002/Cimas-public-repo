using Cimas.Domain.Entities.Cinemas;

namespace Cimas.Application.Interfaces
{
    public interface ICinemaRepository : IBaseRepository<Cinema>
    {
        Task<List<Cinema>> GetCinemasByCompanyIdAsync(Guid companyId);
        Task<Cinema> GetCinemaIncludedProductsByIdAsync(Guid cinemaId);
        Task<Cinema> GetCinemaIncludedAllDependentDataByIdAsync(Guid cinemaId);
    }
}
