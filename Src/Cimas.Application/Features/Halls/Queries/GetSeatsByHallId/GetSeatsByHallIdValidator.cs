using FluentValidation;

namespace Cimas.Application.Features.Halls.Queries.GetSeatsByHallId
{
    public class GetSeatsByHallIdValidator : AbstractValidator<GetSeatsByHallIdQuery>
    {
        public GetSeatsByHallIdValidator()
        {
            RuleFor(x => x.HallId)
               .NotEmpty();
        }
    }
}
