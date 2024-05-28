using Cimas.Domain.Entities.Reports;

namespace Cimas.Api.Contracts.Reports
{
    public record ReportResponse(
        Guid Id,
        DateTime StartDateTime,
        DateTime EndDateTime,
        ReportStatus Status
    );
}
