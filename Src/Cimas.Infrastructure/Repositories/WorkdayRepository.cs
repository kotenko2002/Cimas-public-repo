using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.WorkDays;
using Cimas.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Cimas.Infrastructure.Repositories;

public class WorkdayRepository : BaseRepository<Workday>, IWorkdayRepository
{
    public WorkdayRepository(CimasDbContext context) : base(context)
    {
    }

    public async Task<Workday> GetWorkdayByUserIdAsync(Guid userId)
    {
        return await Sourse
            .Include(workday => workday.User)
            .FirstOrDefaultAsync(workday => workday.UserId == userId && !workday.EndDateTime.HasValue);
    }

    public async Task<Workday> GetNotFinishedWorkdayByCinemaIdAsync(Guid cinemaId)
    {
        return await Sourse
            .FirstOrDefaultAsync(item => item.CinemaId == cinemaId && item.EndDateTime == null);
    }

    public async Task<Workday> GetWorkdayWithBaseDataForReportByIdAsync(Guid workdayId)
    {
        return await Sourse
            .Include(workday => workday.User)
            .Include(workday => workday.Cinema)
                .ThenInclude(workday => workday.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(workday => workday.Id == workdayId);
    }
}
