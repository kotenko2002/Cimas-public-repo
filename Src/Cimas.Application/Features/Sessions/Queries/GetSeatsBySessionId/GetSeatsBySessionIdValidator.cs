using FluentValidation;

namespace Cimas.Application.Features.Sessions.Queries.GetSeatsBySessionId
{
    public class GetSeatsBySessionIdValidator : AbstractValidator<GetSeatsBySessionIdQuery>
    {
        public GetSeatsBySessionIdValidator()
        {
            RuleFor(x => x.SessionId)
               .NotEmpty();
        }
    }
}
