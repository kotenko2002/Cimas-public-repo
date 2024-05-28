using Cimas.Api.Common.Extensions;
using Cimas.Application.Features.Halls.Commands.CreateHall;
using Cimas.Application.Features.Halls.Commands.DeleteHall;
using Cimas.Application.Features.Halls.Commands.UpdateHallSeats;
using Cimas.Application.Features.Halls.Queries.GetHallsByCinemaId;
using Cimas.Api.Contracts.Halls;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cimas.Application.Features.Halls.Queries.GetSeatsByHallId;

namespace Cimas.Api.Controllers
{
    [Route("halls")]
    public class HallController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HallController(
          IMediator mediator,
          IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("{cinemaId}"), Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> CreateHall(Guid cinemaId, CreateHallRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, cinemaId, request).Adapt<CreateHallCommand>();
            ErrorOr<Success> createHallResult = await _mediator.Send(command);

            return createHallResult.Match(
                NoContent,
                Problem
            );
        }

        [HttpGet("{cinemaId}"), Authorize(Roles = $"{Roles.Owner},{Roles.Worker}")]
        public async Task<IActionResult> GetHallsByCinemaId(Guid cinemaId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new GetHallsByCinemaIdQuery(userIdResult.Value, cinemaId);
            ErrorOr<List<Hall>> getHallsResult = await _mediator.Send(command);

            return getHallsResult.Match(
                halls => Ok(halls.Adapt<List<HallResponse>>()),
                Problem
            );
        }

        [HttpGet("seats/{hallId}"), Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> GetSeatsByHallId(Guid hallId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new GetSeatsByHallIdQuery(userIdResult.Value, hallId);
            ErrorOr<List<HallSeat>> getSeatsResult = await _mediator.Send(command);

            return getSeatsResult.Match(
                halls => Ok(halls.Adapt<List<SeatResponse>>()),
                Problem
            );
        }

        [HttpPatch("{hallId}"), Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> UpdateHallSeats(Guid hallId, UpdateHallSeatsRequst requst)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, hallId, requst).Adapt<UpdateHallSeatsCommand>();
            ErrorOr<Success> updateHallSeatsResult = await _mediator.Send(command);

            return updateHallSeatsResult.Match(
                NoContent,
                Problem
            );
        }

        [HttpDelete("{hallId}"), Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> DeleteHall(Guid hallId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new DeleteHallCommand(userIdResult.Value, hallId);
            ErrorOr<Success> deleteHallResult = await _mediator.Send(command);

            return deleteHallResult.Match(
                NoContent,
                Problem
            );
        }
    }
}
