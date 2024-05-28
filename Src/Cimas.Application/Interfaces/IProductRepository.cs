using Cimas.Domain.Entities.Products;

namespace Cimas.Application.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductIncludedCinemaByIdAsync(Guid productId);
        Task<List<Product>> GetProductsByIdsAsync(List<Guid> ids);
        Task<List<Product>> GetProductsByCinemaId(Guid cinemaId);
    }
}
