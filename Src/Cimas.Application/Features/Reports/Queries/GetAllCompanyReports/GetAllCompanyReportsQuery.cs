using Cimas.Domain.Entities.Reports;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Queries.GetAllCompanyReports
{
    public record GetAllCompanyReportsQuery(
        Guid UserId    
    ) : IRequest<ErrorOr<List<Report>>>;
}
