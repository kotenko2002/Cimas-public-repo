using FluentValidation;

namespace Cimas.Application.Features.Auth.Commands.RefreshTokens
{
    public class RefreshTokensValidator : AbstractValidator<RefreshTokensCommand>
    {
        public RefreshTokensValidator()
        {
            RuleFor(x => x.AccessToken)
             .NotEmpty();

            RuleFor(x => x.RefreshToken)
            .NotEmpty();
        }
    }
}
