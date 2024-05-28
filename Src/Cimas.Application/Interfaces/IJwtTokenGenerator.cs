using Cimas.Domain.Models.Auth;
using ErrorOr;
using System.Security.Claims;

namespace Cimas.Application.Interfaces
{
    public interface IJwtTokensService
    {
        AuthModel GenerateTokens(List<Claim> authClaims);
        ErrorOr<ClaimsPrincipal> GetPrincipalFromExpiredToken(string accessToken);
    }
}
