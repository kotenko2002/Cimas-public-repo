using Cimas.Api.Common.Extensions;
using Cimas.Api.Contracts.Reports;
using Cimas.Application.Features.Reports.Commands.UpdateReportStatus;
using Cimas.Application.Features.Reports.Queries.GetAllCompanyReports;
using Cimas.Application.Features.Reports.Queries.GetReportById;
using Cimas.Application.Features.Reports.Queries.GetReportFile;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cimas.Api.Controllers
{
    [Route("reports"), Authorize(Roles = Roles.Reviewer)]
    public class ReportController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanyReports()
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var query = new GetAllCompanyReportsQuery(userIdResult.Value);
            ErrorOr<List<Report>> getAllCompanyReports = await _mediator.Send(query);

            return getAllCompanyReports.Match(
                products => Ok(products.Adapt<List<ReportResponse>>()),
                Problem
            );
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetReportById(Guid reportId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var query = new GetReportByIdQuery(userIdResult.Value, reportId);
            ErrorOr<Report> getReportById = await _mediator.Send(query);

            return getReportById.Match(
                products => Ok(products.Adapt<ReportResponse>()),
                Problem
            );
        }

        [HttpGet("file/{reportId}")]
        public async Task<IActionResult> GetReportFile(Guid reportId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var query = new GetReportFileQuery(userIdResult.Value, reportId);
            ErrorOr<FileDownloadResult> getReportFileResult = await _mediator.Send(query);

            return getReportFileResult.Match(
                report => File(
                    getReportFileResult.Value.Stream,
                    getReportFileResult.Value.ContentType,
                    getReportFileResult.Value.FileName),
                Problem
            );
        }

        [HttpPatch("{reportId}")]
        public async Task<IActionResult> UpdateReportStatus(Guid reportId, UpdateReportStatusRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new UpdateReportStatusCommand(userIdResult.Value, reportId, request.Status);
            ErrorOr<Success> updateReportStatusResult = await _mediator.Send(command);

            return updateReportStatusResult.Match(
                NoContent,
                Problem
            );
        }
    }
}
