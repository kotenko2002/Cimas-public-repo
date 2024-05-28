using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Queries.GetReportById
{
    public class GetReportByIdHandler : IRequestHandler<GetReportByIdQuery, ErrorOr<Report>>
    {
        private readonly IUnitOfWork _uow;

        public GetReportByIdHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Report>> Handle(GetReportByIdQuery query, CancellationToken cancellationToken)
        {
            Report report = await _uow.ReportRepository.GetReportIncludedWorkdayThenIncludedCinemaByIdAsync(query.ReportId);
            if (report is null)
            {
                return Error.NotFound(description: "Report with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(query.UserId);
            if (user.CompanyId != report.WorkDay.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            return report;
        }
    }
}
