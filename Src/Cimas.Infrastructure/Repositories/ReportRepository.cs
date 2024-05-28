using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Reports;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories
{
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(CimasDbContext context) : base(context)
        {
        }

        public async Task<Report> GetReportIncludedWorkdayThenIncludedCinemaByIdAsync(Guid reportId)
        {
            return await Sourse
                .Include(report => report.WorkDay)
                    .ThenInclude(workday => workday.Cinema)
                .FirstOrDefaultAsync(report => report.Id == reportId);
        }

        public async Task<List<Report>> GetReportsByCompanyIdAsync(Guid companyId)
        {
            return await Sourse
                .Include(report => report.WorkDay)
                    .ThenInclude(workday => workday.Cinema)
                .Where(report => report.WorkDay.Cinema.CompanyId == companyId)
                .ToListAsync();
        }
    }
}
