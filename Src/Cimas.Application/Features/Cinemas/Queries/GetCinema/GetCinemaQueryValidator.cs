using FluentValidation;

namespace Cimas.Application.Features.Cinemas.Queries.GetCinema
{
    public class GetCinemaQueryValidator : AbstractValidator<GetCinemaQuery>
    {
        public GetCinemaQueryValidator()
        {
            RuleFor(x => x.CinemaId)
                .NotEmpty();
        }
    }
}
