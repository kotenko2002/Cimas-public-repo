namespace Cimas.Api.Contracts.Products
{
    public record CreateProductRequest(
        string Name,
        decimal Price
    );
}
