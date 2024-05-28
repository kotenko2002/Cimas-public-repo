using Cimas.Domain.Entities.Reports;

namespace Cimas.Application.Interfaces
{
    public interface IReportRepository : IBaseRepository<Report>
    {
        Task<Report> GetReportIncludedWorkdayThenIncludedCinemaByIdAsync(Guid reportId);
        Task<List<Report>> GetReportsByCompanyIdAsync(Guid companyId);
    }
}
