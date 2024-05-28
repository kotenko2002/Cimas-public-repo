using Cimas.Domain.Models;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Queries.GetReportFile
{
    public record GetReportFileQuery(
        Guid UserId,
        Guid ReportId
    ) : IRequest<ErrorOr<FileDownloadResult>>;
}
