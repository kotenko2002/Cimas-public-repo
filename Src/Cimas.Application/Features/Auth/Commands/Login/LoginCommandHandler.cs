using Microsoft.AspNetCore.Identity;
using ErrorOr;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Models.Auth;

namespace Cimas.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AuthModel>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokensService _jwtTokensService;

        public LoginCommandHandler(
            UserManager<User> userManager,
            IJwtTokensService jwtTokensService)
        {
            _userManager = userManager;
            _jwtTokensService = jwtTokensService;
        }

        public async Task<ErrorOr<AuthModel>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByNameAsync(command.Username);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, command.Password))
            {
                return Error.Unauthorized(description: "Неправильне користувацьке ім'я або пароль");
            }

            if (user.IsFired)
            {
                return Error.Unauthorized(description: "Вас було звільнено. Ви більше не маєте доступу до робочого обікового заспису");
            }

            List<Claim> authClaims = await GenerateAuthClaims(user);
            AuthModel model = _jwtTokensService.GenerateTokens(authClaims);

            user.RefreshToken = model.RefreshToken.Value;
            user.RefreshTokenExpiryTime = model.RefreshToken.ValidTo;
            await _userManager.UpdateAsync(user);

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            model.User = user;
            model.Roles = userRoles.ToArray();

            return model;
        }

        private async Task<List<Claim>> GenerateAuthClaims(User user)
        {
            var authClaims = new List<Claim>
            {
                new("userId", user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return authClaims;
        }
    }
}
