using Cimas.Api.Common.Extensions;
using Cimas.Api.Contracts.Workdays;
using Cimas.Application.Features.Workdays.Commands.FinishWorkday;
using Cimas.Application.Features.Workdays.Commands.StartWorkday;
using Cimas.Application.Features.Workdays.Queries.GetCurrentWorkday;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Entities.WorkDays;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cimas.Api.Controllers
{
    [Route("workdays"), Authorize(Roles = Roles.Worker)]
    public class WorkdayController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkdayController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("start/{cinemaId}")]
        public async Task<IActionResult> StartWorkday(Guid cinemaId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new StartWorkdayCommand(userIdResult.Value, cinemaId);
            ErrorOr<Workday> startWorkdayResult = await _mediator.Send(command);

            return startWorkdayResult.Match(
                workday => Ok(workday.Adapt<WorkdayResponse>()),
                Problem
            );
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWorkday()
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new GetCurrentWorkdayQuery(userIdResult.Value);
            ErrorOr<Workday> getCurrentWorkdayResult = await _mediator.Send(command);

            return getCurrentWorkdayResult.Match(
                workday => workday is not null
                    ? Ok(workday.Adapt<WorkdayResponse>())
                    : NoContent(),
                Problem
            );
        }

        [HttpPatch("finish")]
        public async Task<IActionResult> FinishWorkday()
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new FinishWorkdayCommand(userIdResult.Value);
            ErrorOr<Success> finishWorkdayResult = await _mediator.Send(command);

            return finishWorkdayResult.Match(
                NoContent,
                Problem
            );
        }
    }
}
