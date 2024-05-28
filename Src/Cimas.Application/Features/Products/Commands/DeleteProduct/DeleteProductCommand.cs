using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(
        Guid UserId,
        Guid ProductId
    ) : IRequest<ErrorOr<Success>>;
}
