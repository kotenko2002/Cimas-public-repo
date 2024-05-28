using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models.Auth;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Cimas.Application.Features.Auth.Commands.RefreshTokens
{
    public class RefreshTokensHandler : IRequestHandler<RefreshTokensCommand, ErrorOr<AuthModel>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokensService _jwtTokensService;

        public RefreshTokensHandler(
            UserManager<User> userManager,
            IJwtTokensService jwtTokensService)
        {
            _userManager = userManager;
            _jwtTokensService = jwtTokensService;
        }

        public async Task<ErrorOr<AuthModel>> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
        {
            ErrorOr<ClaimsPrincipal> getPrincipalResult = 
                _jwtTokensService.GetPrincipalFromExpiredToken(command.AccessToken);
            if(getPrincipalResult.IsError)
            {
                return getPrincipalResult.Errors;
            }

            // TODO: make getting user By Id insted if by Name
            string username = getPrincipalResult.Value.Identity.Name;
            User user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != command.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Error.Unauthorized(description: "Invalid access token or refresh token");
            }

            List<Claim> authClaims = getPrincipalResult.Value.Claims.ToList();
            AuthModel model = _jwtTokensService.GenerateTokens(authClaims);

            user.RefreshToken = model.RefreshToken.Value;
            user.RefreshTokenExpiryTime = model.RefreshToken.ValidTo;
            await _userManager.UpdateAsync(user);

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            model.User = user;
            model.Roles = userRoles.ToArray();

            return model;
        }
    }
}
