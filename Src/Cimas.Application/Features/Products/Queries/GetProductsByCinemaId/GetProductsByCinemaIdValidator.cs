using FluentValidation;

namespace Cimas.Application.Features.Products.Queries.GetProductsByCinemaId
{
    public class GetProductsByCinemaIdValidator : AbstractValidator<GetProductsByCinemaIdQuery>
    {
        public GetProductsByCinemaIdValidator()
        {
            RuleFor(x => x.CinemaId)
                .NotEmpty();
        }
    }
}
