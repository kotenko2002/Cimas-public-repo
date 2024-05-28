using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Commands.UpdateReportStatus
{
    public class UpdateReportStatusHandler : IRequestHandler<UpdateReportStatusCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _uow;

        public UpdateReportStatusHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateReportStatusCommand command, CancellationToken cancellationToken)
        {
            Report report = await _uow.ReportRepository.GetReportIncludedWorkdayThenIncludedCinemaByIdAsync(command.ReportId);
            if (report is null)
            {
                return Error.NotFound(description: "Report with such id does not exist");
            }

            User user = await _uow.UserRepository.GetByIdAsync(command.UserId);
            if (user.CompanyId != report.WorkDay.Cinema.CompanyId)
            {
                return Error.Forbidden(description: "You do not have the necessary permissions to perform this action");
            }

            report.Status = command.Status;

            await _uow.CompleteAsync();

            return Result.Success;
        }
    }
}
