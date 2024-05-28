using Cimas.Api.Common.Extensions;
using Cimas.Api.Contracts.Tickets;
using Cimas.Application.Features.Tickets.Commands.CreateTickets;
using Cimas.Application.Features.Tickets.Commands.DeleteTickets;
using Cimas.Application.Features.Tickets.Commands.GenerateTicketsFile;
using Cimas.Application.Features.Tickets.Commands.UpdateTickets;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cimas.Api.Controllers
{
    [Route("tickets"), Authorize(Roles = Roles.Worker)]
    public class TicketController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("{sessionId}")]
        public async Task<IActionResult> CreateTicket(Guid sessionId, CreateTicketsRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, sessionId, request).Adapt<CreateTicketsCommand>();
            ErrorOr<List<Guid>> createTicketResult = await _mediator.Send(command);

            return createTicketResult.Match(
                Ok,
                Problem
            );
        }

        [HttpPost("file/generate")]
        public async Task<IActionResult> GenerateTicketsFile(GenerateTicketsFileRequest request)
        {
            var command = new GenerateTicketsFileCommand(request.IdsOfSoldTickets);
            ErrorOr<FileDownloadResult> createTicketResult = await _mediator.Send(command);

            return createTicketResult.Match(
                  ticketsFile => File(
                    ticketsFile.Stream,
                    ticketsFile.ContentType,
                    ticketsFile.FileName),
                Problem
            );
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateTickets(UpdateTicketsRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, request).Adapt<UpdateTicketsCommand>();
            ErrorOr<List<Guid>> updateTicketsResult = await _mediator.Send(command);

            return updateTicketsResult.Match(
                Ok,
                Problem
            );
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTicket(DeleteTicketsRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new DeleteTicketsCommand(userIdResult.Value, request.TikectIds);
            ErrorOr<Success> createTicketResult = await _mediator.Send(command);

            return createTicketResult.Match(
                NoContent,
                Problem
            );
        }
    }
}
