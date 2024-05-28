using Cimas.Application.Features.Auth.Commands.Login;
using Cimas.Application.Features.Auth.Commands.RefreshTokens;
using Cimas.Api.Contracts.Auth;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Cimas.Application.Features.Auth.Commands.RegisterOwner;
using Cimas.Domain.Models.Auth;
using Cimas.Api.Contracts;

namespace Cimas.Api.Controllers
{
    [Route("auth")]
    public class AuthController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register/owner")]
        public async Task<IActionResult> RegisterOwner(RegisterOwnerRequest request)
        {
            var command = request.Adapt<RegisterOwnerCommand>();
            ErrorOr<Success> registerOwnerResult = await _mediator.Send(command);

            return registerOwnerResult.Match(
                NoContent,
                Problem
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = request.Adapt<LoginCommand>();

            ErrorOr<AuthModel> loginResult = await _mediator.Send(command);

            return loginResult.Match(
                result =>
                {
                    AppendRefreshTokenToResponse(result.RefreshToken);

                    return Ok(result.Adapt<AuthResponse>());
                },
                Problem
            );
        }

        [HttpPost("refresh-tokens")]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensRequest request)
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token is missing");
            }

            var command = new RefreshTokensCommand(request.AccessToken, refreshToken);

            ErrorOr<AuthModel> refreshTokensResult = await _mediator.Send(command);

            return refreshTokensResult.Match(
                result =>
                {
                    AppendRefreshTokenToResponse(result.RefreshToken);

                    return Ok(result.Adapt<AuthResponse>());
                },
                Problem
            );
        }

        private void AppendRefreshTokenToResponse(Token refreshToken)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.ValidTo,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken.Value, options);
        }
    }
}
