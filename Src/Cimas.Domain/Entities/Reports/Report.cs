using Cimas.Domain.Entities.WorkDays;

namespace Cimas.Domain.Entities.Reports
{
    public class Report : BaseEntity
    {
        public string FileId { get; set; }
        public ReportStatus Status { get; set; }

        public Guid WorkDayId { get; set; }
        public virtual Workday WorkDay { get; set; }
    }
}
