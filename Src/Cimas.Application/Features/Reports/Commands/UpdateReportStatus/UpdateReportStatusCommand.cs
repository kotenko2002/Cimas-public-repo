using Cimas.Domain.Entities.Reports;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Commands.UpdateReportStatus
{
    public record UpdateReportStatusCommand(
        Guid UserId,
        Guid ReportId,
        ReportStatus Status
    ) : IRequest<ErrorOr<Success>>;
}
