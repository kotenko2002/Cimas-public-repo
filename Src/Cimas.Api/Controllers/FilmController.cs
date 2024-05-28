using Cimas.Api.Common.Extensions;
using Cimas.Api.Contracts.Films;
using Cimas.Application.Features.Films.Commands.CreateFilm;
using Cimas.Application.Features.Films.Commands.DeleteFilm;
using Cimas.Application.Features.Films.Queries.GetFilmsByCinemaId;
using Cimas.Domain.Entities.Films;
using Cimas.Domain.Entities.Users;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cimas.Api.Controllers
{
    [Route("films"), Authorize(Roles = Roles.Worker)]
    public class FilmController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FilmController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("{cinemaId}")]
        public async Task<IActionResult> CreateFilm(Guid cinemaId, CreateFilmRequest request)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = (userIdResult.Value, cinemaId, request).Adapt<CreateFilmCommand>();
            ErrorOr<Success> createFilmResult = await _mediator.Send(command);

            return createFilmResult.Match(
                NoContent,
                Problem
            );
        }

        [HttpGet("{cinemaId}")]
        public async Task<IActionResult> GetFilmsByCinemaId(Guid cinemaId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var query = new GetFilmsByCinemaIdQuery(userIdResult.Value, cinemaId);
            ErrorOr<List<Film>> getFilmsResult = await _mediator.Send(query);

            return getFilmsResult.Match(
                halls => Ok(halls.Adapt<List<FilmResponse>>()),
                Problem
            );
        }
        
        [HttpDelete("{filmId}")]
        public async Task<IActionResult> DeleteFilm(Guid filmId)
        {
            ErrorOr<Guid> userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userIdResult.IsError)
            {
                return Problem(userIdResult.Errors);
            }

            var command = new DeleteFilmCommand(userIdResult.Value, filmId);
            ErrorOr<Success> deleteFilmResult = await _mediator.Send(command);

            return deleteFilmResult.Match(
                NoContent,
                Problem
            );
        }
    }
}
