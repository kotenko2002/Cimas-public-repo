using Cimas.Domain.Entities.WorkDays;

namespace Cimas.Application.Interfaces
{
    public interface IWorkdayRepository : IBaseRepository<Workday>
    {
        Task<Workday> GetWorkdayByUserIdAsync(Guid userId);
        Task<Workday> GetNotFinishedWorkdayByCinemaIdAsync(Guid cinemaId);
        Task<Workday> GetWorkdayWithBaseDataForReportByIdAsync(Guid workdayId);
    }
}
