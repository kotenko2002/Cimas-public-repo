using FluentValidation;

namespace Cimas.Application.Features.Films.Queries.GetFilmsByCinemaId
{
    public class GetFilmsByCinemaIdValidator : AbstractValidator<GetFilmsByCinemaIdQuery>
    {
        public GetFilmsByCinemaIdValidator()
        {
            RuleFor(x => x.CinemaId)
               .NotEmpty();
        }
    }
}
