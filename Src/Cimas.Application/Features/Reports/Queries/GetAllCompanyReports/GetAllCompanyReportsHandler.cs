using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Queries.GetAllCompanyReports
{
    public class GetAllCompanyReportsHandler : IRequestHandler<GetAllCompanyReportsQuery, ErrorOr<List<Report>>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllCompanyReportsHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<List<Report>>> Handle(GetAllCompanyReportsQuery query, CancellationToken cancellationToken)
        {
            User user = await _uow.UserRepository.GetByIdAsync(query.UserId);

            return await _uow.ReportRepository.GetReportsByCompanyIdAsync(user.CompanyId);
        }
    }
}
