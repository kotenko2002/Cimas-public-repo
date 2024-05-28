using ErrorOr;
using MediatR;

namespace Cimas.Application.Features.Products.Commands.UpateProduct
{
    public record UpateProductsCommand(
        Guid UserId,
        List<UpdateProductModel> Products
    ) : IRequest<ErrorOr<Success>>;

    public record UpdateProductModel(
        Guid Id,
        string Name,
        decimal Price,
        int Amount,
        int SoldAmount,
        int IncomeAmount
    );
}
