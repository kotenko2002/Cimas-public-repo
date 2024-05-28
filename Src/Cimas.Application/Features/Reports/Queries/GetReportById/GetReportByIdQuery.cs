using Cimas.Domain.Entities.Reports;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Queries.GetReportById
{
    public record GetReportByIdQuery(
        Guid UserId,
        Guid ReportId
    ) : IRequest<ErrorOr<Report>>;
}
