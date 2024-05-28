namespace Cimas.Api.Contracts.Products
{
    public record UpdateProductsRequest(
        List<UpdateProductRequestModel> Products
    );

    public record UpdateProductRequestModel(
        Guid Id,
        string Name,
        decimal Price,
        int Amount,
        int SoldAmount,
        int IncomeAmount
    );
}
