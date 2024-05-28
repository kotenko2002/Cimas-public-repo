using Cimas.Api.Common.Extensions;
using Cimas.Api.Contracts.Sessions;
using Cimas.Application.Features.Sessions.Commands.CreateSession;
using Cimas.Application.Features.Sessions.Commands.DeleteSession;
using Cimas.Application.Features.Sessions.Queries.GetSeatsBySessionId;
using Cimas.Application.Features.Sessions.Queries.GetSessionsByRange;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models.Sessions;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cimas.Api.Controllers
{
    [Route("sessions"), Authorize(Roles = Roles.Worker)]
    public class SessionController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(CreateSessionRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, request).Adapt<CreateSessionCommand>();
            ErrorOr<Success> createSessionResult = await _mediator.Send(command);

            return createSessionResult.Match(
                NoContent,
                Problem
            );
        }

        [HttpGet("byRange")]
        public async Task<IActionResult> GetSessionsByRange([FromQuery] GetSessionsByRangeRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var query = (userIdResult.Value, request).Adapt<GetSessionsByRangeQuery>();
            ErrorOr<List<Session>> getSessionsByRangeResult = await _mediator.Send(query);

            return getSessionsByRangeResult.Match(
                sessions => Ok(sessions.Adapt<List<SessionResponse>>()),
                Problem
            );
        }

        [HttpGet("seats/{sessionId}")]
        public async Task<IActionResult> GetSeatsBySessionId(Guid sessionId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var query = new GetSeatsBySessionIdQuery(userIdResult.Value, sessionId);
            ErrorOr<List<SessionSeat>> getSeatsBySessionIdResult = await _mediator.Send(query);

            return getSeatsBySessionIdResult.Match(
                Ok,
                Problem
            );
        }

        [HttpDelete("{sessionId}")]
        public async Task<IActionResult> DeleteSession(Guid sessionId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new DeleteSessionCommand(userIdResult.Value, sessionId);
            ErrorOr<Success> deleteSessionResult = await _mediator.Send(command);

            return deleteSessionResult.Match(
                NoContent,
                Problem
            );
        }
    }
}
