using ErrorOr;
using System.Security.Claims;

namespace Cimas.Api.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static ErrorOr<Guid> GetUserId(this ClaimsPrincipal principal)
        {
            var getUserIdResult = GetInfoByDataName(principal, "userId");
            if (getUserIdResult.IsError)
            {
                return getUserIdResult.Errors;
            }

            if (Guid.TryParse(getUserIdResult.Value, out Guid guid))
            {
                return guid;
            }

            return Error.Failure(description: $"Wrong Id format in token");
        }

        private static ErrorOr<string> GetInfoByDataName(ClaimsPrincipal principal, string name)
        {
            var data = principal.FindFirstValue(name);

            if (data is null)
            {
                return Error.NotFound(description: $"No such data as {name} in token");
            }

            return data;
        }
    }
}
