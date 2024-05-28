using Cimas.Domain.Entities.Reports;

namespace Cimas.Api.Contracts.Reports
{
    public record UpdateReportStatusRequest(
        ReportStatus Status    
    );
}
