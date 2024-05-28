using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
        Guid UserId,
        Guid CinemaId,
        string Name,
        decimal Price
    ) : IRequest<ErrorOr<Success>>;
}
