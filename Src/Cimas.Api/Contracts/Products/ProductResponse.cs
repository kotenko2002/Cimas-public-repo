namespace Cimas.Api.Contracts.Products
{
    public record ProductResponse(
        Guid Id,
        string Name,
        decimal Price,
        int Amount,
        int SoldAmount,
        int IncomeAmount
    );
}
