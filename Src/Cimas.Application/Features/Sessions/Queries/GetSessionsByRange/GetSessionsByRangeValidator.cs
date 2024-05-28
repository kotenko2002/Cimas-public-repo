using FluentValidation;

namespace Cimas.Application.Features.Sessions.Queries.GetSessionsByRange
{
    public class GetSessionsByRangeValidator : AbstractValidator<GetSessionsByRangeQuery>
    {
        public GetSessionsByRangeValidator()
        {
            RuleFor(x => x.CinemaId)
               .NotEmpty();

            RuleFor(x => x.FromDateTime)
               .NotEmpty();

            RuleFor(x => x.ToDateTime)
               .NotEmpty();
        }
    }
}
