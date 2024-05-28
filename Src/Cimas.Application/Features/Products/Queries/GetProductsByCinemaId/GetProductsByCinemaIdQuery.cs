using Cimas.Domain.Entities.Products;
using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Queries.GetProductsByCinemaId
{
    public record GetProductsByCinemaIdQuery(
        Guid UserId,
        Guid CinemaId
    ) : IRequest<ErrorOr<List<Product>>>;
}
