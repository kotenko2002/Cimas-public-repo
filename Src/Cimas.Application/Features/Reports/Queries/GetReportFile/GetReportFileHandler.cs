using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Reports.Queries.GetReportFile
{
    public class GetReportFileHandler : IRequestHandler<GetReportFileQuery, ErrorOr<FileDownloadResult>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _fileStorageService;

        public GetReportFileHandler(
            IUnitOfWork uow,
            IFileStorageService fileStorageService)
        {
            _uow = uow;
            _fileStorageService = fileStorageService;
        }

        public async Task<ErrorOr<FileDownloadResult>> Handle(GetReportFileQuery query, CancellationToken cancellationToken)
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

            ErrorOr<FileDownloadResult> downloadFileResult = await _fileStorageService.DownloadFileAsync(report.FileId);

            if (downloadFileResult.IsError)
            {
                return downloadFileResult.Errors;
            }

            return downloadFileResult.Value;
        }
    }
}
